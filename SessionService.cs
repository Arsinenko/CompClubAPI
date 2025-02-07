using System.Collections.Concurrent;
using CompClubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI;

public class SessionService
{
    private readonly ConcurrentDictionary<int, Timer> _timers = new();
    private readonly IServiceScopeFactory _scopeFactory;

    public SessionService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task<int> StartTimer(int interval, int accountId)
    {
        Timer timer = new Timer(async void (state) => await DoWork(state), accountId, TimeSpan.Zero,
            TimeSpan.FromSeconds(interval));
        _timers[accountId] = timer;
        Console.WriteLine($"timer {accountId} started");
        return Task.FromResult(accountId);

    }

    public async Task<bool> StopTimer(int accountId)
    {
        if (_timers.TryRemove(accountId, out Timer? timer))
        {
            await timer.DisposeAsync();
            await StopBooking(accountId);
            Console.WriteLine($"timer {accountId} stopped");
            return true;
        }

        return false;
    }

    public async Task DoWork(object? state)
    {
        int accountId = (int)state!;

        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CollegeTaskContext>();

            Account? account = await context.Accounts.FindAsync(accountId);

            if (account == null)
            {
                Console.WriteLine("account not found");
            }
            else
            {
                var tariff = await context.Bookings
                    .Where(b => b.AccountId == accountId && b.IdStatus == 1)
                    .Select(b => b.IdWorkingSpace)
                    .Select(wsId => context.WorkingSpaces
                        .Where(ws => ws.Id == wsId)
                        .Select(ws => ws.Tariff)
                        .FirstOrDefault())
                    .FirstOrDefaultAsync();

                if (tariff != null && account.Balance - tariff.PricePerMinute >= 0)
                {
                    decimal? previousBalance = account.Balance; // For balanceHistory
                    account.Balance -= tariff.PricePerMinute;
                    context.Accounts.Update(account);
                    await context.SaveChangesAsync();

                    Booking booking =
                        (await context.Bookings.Where(b => b.AccountId == accountId).FirstOrDefaultAsync())!;
                    booking.TotalCost += tariff.PricePerMinute;
                    context.Bookings.Update(booking);
                    await context.SaveChangesAsync();

                    BalanceHistory history = new BalanceHistory
                    {
                        Action = "spent balance for session",
                        Price = tariff.PricePerMinute,
                        PreviousBalance = previousBalance,
                        CreatedAt = DateTime.Now,
                        AccountId = accountId
                    };

                    context.BalanceHistories.Add(history);
                    await context.SaveChangesAsync();


                    Console.WriteLine($"{tariff.PricePerMinute}$ spent from account with id " + accountId);
                }
                else
                {
                    Console.WriteLine("no enough money in account with id " + accountId);

                    await StopBooking(accountId);
                    await StopTimer(accountId);
                }
            }
        }
    }

    async Task StopBooking(int accountId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CollegeTaskContext>();
            Booking booking = (await context.Bookings.Where(b => b.AccountId == accountId).FirstOrDefaultAsync())!;
            booking.EndTime = DateTime.Now;
            booking.IdStatus = 2;
            booking.UpdatedAt = DateTime.Now;

            context.Update(booking);
            await context.SaveChangesAsync();
        }

    }
}