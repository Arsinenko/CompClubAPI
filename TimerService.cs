using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using CompClubAPI.Models;

namespace CompClubAPI;

public class TimerService
{
    private readonly ConcurrentDictionary<int, Timer> _timers = new();
    private readonly IServiceScopeFactory _scopeFactory; // Используем IServiceScopeFactory

    public TimerService(IServiceScopeFactory scopeFactory) // Передаем IServiceScopeFactory в конструктор
    {
        _scopeFactory = scopeFactory;
    }

    public int StartTimer(int interval, int accountId)
    {
        Timer timer = new Timer(DoWork, accountId, TimeSpan.Zero, TimeSpan.FromSeconds(interval));
        _timers[accountId] = timer;
        Console.WriteLine($"timer {accountId} started");
        return accountId;
    }

    public bool StopTimer(int accountId)
    {
        if (_timers.TryRemove(accountId, out Timer? timer))
        {
            timer.Dispose();
            Console.WriteLine($"timer {accountId} stopped");
            return true;
        }

        return false;
    }

    public void DoWork(object? state)
    {
        int accountId = (int)state;

        using (var scope = _scopeFactory.CreateScope()) // Создаем новый Scope
        {
            var context = scope.ServiceProvider.GetRequiredService<CollegeTaskContext>();

            Account? account = context.Accounts.Find(accountId);

            if (account == null)
            {
                Console.WriteLine("account not found");
            }
            else
            {
                if (account.Balance - 8 >= 0)
                {
                    account.Balance -= 8;
                    context.Accounts.Update(account);
                    context.SaveChanges();
                    Console.WriteLine("8$ spent from account with id " + accountId);
                }
                else
                {
                    Console.WriteLine("no enough money in account with id " + accountId);
                }
            }
        }
    }
}
