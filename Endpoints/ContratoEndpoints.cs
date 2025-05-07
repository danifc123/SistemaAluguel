using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Models;
using SistemaAluguel.DTOs;

namespace SistemaAluguel.Endpoints
{
    public static class ContratoEndpoints
    {
        public static void MapContratoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/contratos", async (AppDbContext db) =>
            {
                var contratos = await db.Contratos
                   .Include(c => c.Inquilino)
                   .Include(c => c.Imovel)
                   .Select(c => new ContratoDTO
                   {
                       Id = c.Id,
                       DataInicio = c.DataInicio,
                       DataFim = c.DataFim,
                       ValorMensal = c.ValorMensal,
                       Ativo = c.Ativo,
                       NomeInquilino = c.Inquilino != null ? c.Inquilino.Nome : "N/A",
                       EnderecoImovel = c.Imovel != null ? c.Imovel.Endereco : "N/A"
                   })
                   .ToListAsync();

                return Results.Ok(contratos);
            });

            app.MapPost("/contratos", async (AppDbContext db, Contrato contrato) =>
            {
                db.Contratos.Add(contrato);
                await db.SaveChangesAsync();
                return Results.Created($"/contratos/{contrato.Id}", contrato);
            }).RequireAuthorization();


            app.MapPut("/contratos", async (AppDbContext db, Contrato contrato) =>
            {
                var contratoExistente = await db.Contratos.FindAsync(contrato.Id);

                if (contratoExistente is null)
                    return Results.NotFound($"Contrato com Id {contrato.Id} não encontrado");

                contratoExistente.Ativo = contrato.Ativo;
                contratoExistente.DataFim = contrato.DataFim;
                contratoExistente.DataInicio = contrato.DataInicio;
                contratoExistente.ImovelId = contrato.ImovelId;
                contratoExistente.InquilinoId = contrato.InquilinoId;
                contratoExistente.ValorMensal = contrato.ValorMensal;

                await db.SaveChangesAsync();

                return Results.Ok(contratoExistente);
            }).RequireAuthorization();

            app.MapDelete("/contratos/{id}", async (AppDbContext db, int id) =>
            {
                var contrato = await db.Contratos.FindAsync(id);

                if (contrato is null)
                    return Results.NotFound($"Contrato com o Id {id} não encontrado");

                db.Contratos.Remove(contrato);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapGet("/contratos/{id}", async (AppDbContext db, int id) =>
            {
                var contrato = await db.Contratos
                   .Include(c => c.Inquilino)
                   .Include(c => c.Imovel)
                   .Where(c => c.Id == id)
                   .Select(c => new ContratoDTO
                   {
                       Id = c.Id,
                       DataInicio = c.DataInicio,
                       DataFim = c.DataFim,
                       ValorMensal = c.ValorMensal,
                       Ativo = c.Ativo,
                       NomeInquilino = c.Inquilino != null ? c.Inquilino.Nome : "N/A",
                       EnderecoImovel = c.Imovel != null ? c.Imovel.Endereco : "N/A"
                   })
                   .FirstOrDefaultAsync();

                if (contrato is null)
                    return Results.NotFound($"Contrato com ID {id} não encontrado.");

                return Results.Ok(contrato);
            });
        }
    }
}