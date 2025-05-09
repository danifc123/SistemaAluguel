using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.DTOs;
using SistemaAluguel.Models;

namespace SistemaAluguelAPI.Endpoints
{
    public static class ImovelEndpoints
    {
        public static void MapImovelEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/imoveis", async (AppDbContext db) =>
            {
                var imoveis = await db.Imoveis
                    .Select(i => new ImovelDTO
                    {
                        Id = i.Id,
                        Endereco = i.Endereco,
                        Tipo = i.Tipo,
                        ValorAluguel = i.ValorAluguel,
                        Disponivel = i.Disponivel
                    })
                    .ToListAsync();
                    
                return Results.Ok(imoveis);
            });

            app.MapPost("/imoveis", async (AppDbContext db, Imovel imovel) =>
            {
                db.Imoveis.Add(imovel);
                await db.SaveChangesAsync();
                return Results.Created($"/imoveis/{imovel.Id}", imovel);
            }).RequireAuthorization();

            app.MapPut("/imoveis", async (AppDbContext db, Imovel imovel) =>
            {
                var imovelExistente = await db.Imoveis.FindAsync(imovel.Id);

                if (imovelExistente is null)
                    return Results.NotFound($"Imóvel com ID {imovel.Id} não encontrado.");

                imovelExistente.Endereco = imovel.Endereco;
                imovelExistente.Tipo = imovel.Tipo;
                imovelExistente.ValorAluguel = imovel.ValorAluguel;
                imovelExistente.Disponivel = imovel.Disponivel;

                await db.SaveChangesAsync();

                return Results.Ok(imovelExistente);
            }).RequireAuthorization();

            app.MapDelete("/imoveis/{id}", async (AppDbContext db, int id) =>
            {
                var imovel = await db.Imoveis.FindAsync(id);

                if (imovel is null)
                    return Results.NotFound($"Imóvel com Id {id} não encontrado.");

                db.Imoveis.Remove(imovel);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapGet("/imoveis/{id}", async (AppDbContext db, int id) =>
            {
                var imovel = await db.Imoveis
                    .Where(i => i.Id == id)
                    .Select(i => new ImovelDTO
                    {
                        Id = i.Id,
                        Endereco = i.Endereco,
                        Tipo = i.Tipo,
                        ValorAluguel = i.ValorAluguel,
                        Disponivel = i.Disponivel,
                    })
                    .FirstOrDefaultAsync();

                    if(imovel is null)
                        return Results.NotFound($"ID {id} não encontrado");

                    return Results.Ok(imovel);
            });
        }
    }
}