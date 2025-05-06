using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.DTOs;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class InquilinoEndpoint
    {
        public static void MapInquilinoEndpoint(this IEndpointRouteBuilder app)
        {
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
                var inquilino = await db.Inquilinos
                .Where(i => i.Id == id )
                .Select(i => new InquilinoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Email = i.Email,
                    Telefone = i.Telefone,
                    CPF = i.CPF
                })
                .FirstOrDefaultAsync();

                if(inquilino is null)
                    return Results.NotFound($"ID {id} não encontrado");
                
                return Results.Ok(inquilino);
            });
            app.MapGet("/inquilinos", async(AppDbContext db, string? nome) =>
            {
                var query = db.Inquilinos.AsQueryable();

                if (!string.IsNullOrEmpty(nome))
                {
                    query = query.Where(i => i.Nome!.ToLower().Contains(nome.ToLower()));
                }
                var inquilinos = await query
                    .Select(i => new InquilinoDTO
                    {
                        Id = i.Id,
                        Nome = i.Nome,
                        Email = i.Email,
                        Telefone = i.Telefone,
                        CPF = i.CPF
                    })
                    .ToListAsync();

                return Results.Ok(inquilinos);
            });
            app.MapGet("/inquilinos/ativos", async (AppDbContext db) => 
            {
                var inquilinosAtivos = await db.Contratos
                    .Where(c => c.Ativo)
                    .Include(c => c.Inquilino)
                    .Select(c => new InquilinoDTO
                    {
                        Id = c.Inquilino!.Id,
                        Nome = c.Inquilino.Nome,
                        Email = c.Inquilino.Email,
                        Telefone = c.Inquilino.Telefone,
                        CPF = c.Inquilino.CPF 
                    })
                    .GroupBy(i => i.Id)
                    .Select(g => g.First())
                    .ToListAsync();

                return Results.Ok(inquilinosAtivos);
            });
        }
    }
}
