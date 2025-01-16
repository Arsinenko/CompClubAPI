using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CompClubAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [JsonIgnore] public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore] public virtual ICollection<UserActionLog> UserActionLogs { get; set; } = new List<UserActionLog>();
}


public static class ClientEndpoints
{
	public static void MapClientEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Client").WithTags(nameof(Client));

        group.MapGet("/", async (CollegeTaskContext db) =>
        {
            return await db.Clients.ToListAsync();
        })
        .WithName("GetAllClients")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Client>, NotFound>> (int id, CollegeTaskContext db) =>
        {
            return await db.Clients.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Client model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClientById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Client client, CollegeTaskContext db) =>
        {
            var affected = await db.Clients
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, client.Id)
                  .SetProperty(m => m.Login, client.Login)
                  .SetProperty(m => m.Password, client.Password)
                  .SetProperty(m => m.FirstName, client.FirstName)
                  .SetProperty(m => m.MiddleName, client.MiddleName)
                  .SetProperty(m => m.LastName, client.LastName)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateClient")
        .WithOpenApi();

        group.MapPost("/", async (Client client, CollegeTaskContext db) =>
        {
            db.Clients.Add(client);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Client/{client.Id}",client);
        })
        .WithName("CreateClient")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, CollegeTaskContext db) =>
        {
            var affected = await db.Clients
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteClient")
        .WithOpenApi();
    }
}