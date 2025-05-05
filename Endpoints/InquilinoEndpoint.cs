using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class InquilinoEndpoint
    {
        public static void MapInquilinoEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/inquilino", async (AppDbContext db) => {
                var imoveis = await db.Imoveis.ToListAsync();
                return Results.Ok(imoveis);
            });
            app.MapPost("/inquilino", async (AppDbContext db, Inquilino inquilino) => {
                db.Inquilinos.Add(inquilino);
                await db.SaveChangesAsync();
                return Results.Created($"/inquilino/{inquilino.Id}", inquilino);
            });
            app.MapPut("inquilino", async (AppDbContext db, Inquilino inquilino) => {
                var inquilinoExistente = await db.Inquilinos.FindAsync(inquilino.Id);
                if(inquilinoExistente is null)
                    return Results.NotFound($"Inquilino com ID {inquilino.Id} não encontrado");
                    

                inquilinoExistente.CPF= inquilino.CPF;
                inquilinoExistente.Email = inquilino.Email;
                inquilinoExistente.Nome = inquilino.Nome;
                inquilinoExistente.Telefone = inquilino.Telefone;

                await db.SaveChangesAsync();

                return Results.Ok(inquilinoExistente);
            });
        }
    }
}