using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class ImovelEndpoints
    {
        public static void MapImovelEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/imoveis", async (AppDbContext db) =>
            {
                var imoveis = await db.Imoveis.ToListAsync();
                return Results.Ok(imoveis);
            });

            app.MapPost("/imoveis", async (AppDbContext db, Imovel imovel) =>
            {
                db.Imoveis.Add(imovel);
                await db.SaveChangesAsync();
                return Results.Created($"/imoveis/{imovel.Id}", imovel);
            });

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
            });

            app.MapDelete("/imoveis/{id}", async (AppDbContext db, int id) =>
            {
                var imovel = await db.Imoveis.FindAsync(id);

                if (imovel is null)
                    return Results.NotFound($"Imóvel com Id {id} não encontrado.");

                db.Imoveis.Remove(imovel);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapGet("/imoveis/{id}", async (AppDbContext db, int id) =>
            {
                var imovel = await db.Imoveis.FindAsync(id);
                if (imovel is null)
                    return Results.NotFound($"Imóvel com Id {id} nao Encontrado");
                
                    return Results.Ok(imovel);
            });
        }
    }
}