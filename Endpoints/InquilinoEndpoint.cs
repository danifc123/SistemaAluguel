using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class InquilinoEndpoint
    {
        public static void MapInquilinoEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/inquilinos", async (AppDbContext db) =>
            {
                var inquilinos = await db.Inquilinos.ToListAsync();
                return Results.Ok(inquilinos);
            });

            app.MapPost("/inquilinos", async (AppDbContext db, Inquilino inquilino) =>
            {
                db.Inquilinos.Add(inquilino);
                await db.SaveChangesAsync();
                return Results.Created($"/inquilinos/{inquilino.Id}", inquilino);
            });

            app.MapPut("/inquilinos", async (AppDbContext db, Inquilino inquilino) =>
            {
                var inquilinoExistente = await db.Inquilinos.FindAsync(inquilino.Id);
                if (inquilinoExistente is null)
                    return Results.NotFound($"Inquilino com ID {inquilino.Id} não encontrado.");

                inquilinoExistente.CPF = inquilino.CPF;
                inquilinoExistente.Email = inquilino.Email;
                inquilinoExistente.Nome = inquilino.Nome;
                inquilinoExistente.Telefone = inquilino.Telefone;

                await db.SaveChangesAsync();

                return Results.Ok(inquilinoExistente);
            });

            app.MapDelete("/inquilinos/{id}", async (AppDbContext db, int id) =>
            {
                var inquilino = await db.Inquilinos.FindAsync(id);

                if (inquilino is null)
                    return Results.NotFound($"Inquilino com ID {id} não encontrado.");

                db.Inquilinos.Remove(inquilino);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapGet("/inquilinos/{id}", async (AppDbContext db, int id) =>
            {
                var inquilino = await db.Inquilinos.FindAsync(id);

                if (inquilino is null)
                    return Results.NotFound($"Inquilino com ID {id} não encontrado.");

                return Results.Ok(inquilino);
            });
        }
    }
}
