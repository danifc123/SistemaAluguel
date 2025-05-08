
using System.Security;
using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.DTOs;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class PagamentosEndpoints
    {
        public static void MapPagamentosEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/pagamentos", async (AppDbContext db) =>
            {
                var pagamento = await db.Pagamentos
                    .Select(p => new PagamentoDTO
                    {
                        Id = p.Id,
                        ContratoId = p.ContratoId,
                        MesAnoReferencia = p.MesAnoReferencia,
                        DataVencimento = p.DataVencimento,
                        DataPagamento = p.DataPagamento,
                        Valor = p.Valor,
                        Status = p.StatusPagamento.ToString()
                    })
                    .ToListAsync();
                return Results.Ok(pagamento);
            });
            app.MapPost("/pagamentos", async (AppDbContext db, Pagamento pagamento) =>
            {   
                //verifica se o pagamento já existe
                var pagamentoDuplicado = await db.Pagamentos
                .AnyAsync(p => p.ContratoId == pagamento.ContratoId && p.MesAnoReferencia == pagamento.MesAnoReferencia);
                if (pagamentoDuplicado)
                    return Results.BadRequest("Já existe um pagamento para este contrato e mês de referência.");

                //verifica se o contrato existe
                 var contratoExiste = await db.Contratos.AnyAsync(c => c.Id == pagamento.ContratoId);
                if(!contratoExiste)
                    return Results.BadRequest("Contrato não encontrado");

                db.Pagamentos.Add(pagamento);
                await db.SaveChangesAsync();
                return Results.Created($"/pagamentos/{pagamento.Id}", pagamento);
                
            }).RequireAuthorization();

            app.MapPut("/pagamentos", async (AppDbContext db, Pagamento pagamento) =>
            {
                var pagamentoExistente = await db.Pagamentos.FindAsync(pagamento.Id);

                if (pagamentoExistente is null)
                    return Results.NotFound($"Pagamento com o ID {pagamento.Id} não encontrado");

                pagamentoExistente.ContratoId = pagamento.ContratoId;
                pagamentoExistente.DataPagamento = pagamento.DataPagamento;
                pagamentoExistente.DataVencimento = pagamento.DataVencimento;
                pagamentoExistente.MesAnoReferencia = pagamento.MesAnoReferencia;
                pagamentoExistente.StatusPagamento = pagamento.StatusPagamento;
                pagamentoExistente.Valor = pagamento.Valor;

                await db.SaveChangesAsync();

                return Results.Ok(pagamentoExistente);
            }).RequireAuthorization();
            app.MapDelete("/pagamentos/{id}", async (AppDbContext db, int id) =>
            {
                var pagamento = await db.Pagamentos.FindAsync(id);

                if (pagamento is null)
                    return Results.NotFound($"Pagamento com o ID {id} não encontrado");

                db.Pagamentos.Remove(pagamento);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapGet("/pagamentos/{id}", async (AppDbContext db, int id) =>
            { 
                var pagamento = await db.Pagamentos
                    .Where(p => p.Id == id)
                    .Select(p=> new PagamentoDTO 
                    {
                        Id = p.Id,
                        ContratoId = p.ContratoId,
                        MesAnoReferencia = p.MesAnoReferencia,
                        DataVencimento = p.DataVencimento,
                        DataPagamento = p.DataPagamento,
                        Valor = p.Valor,
                        Status = p.StatusPagamento.ToString()
                    })
                    .FirstOrDefaultAsync();

                    if (pagamento is null)
                        return Results.NotFound($"Pagamento com o ID {id} não encontrado");

                    return Results.Ok(pagamento);
            });
        }
    }
}