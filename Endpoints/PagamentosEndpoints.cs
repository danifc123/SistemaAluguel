
using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.Models;

namespace SistemaAluguel.Endpoints
{
    public static class PagamentosEndpoints
    {
        public static void MapPagamentosEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/pagamentos", async (AppDbContext db) =>
            {
                var pagamento = await db.Pagamentos.ToListAsync();
                return Results.Ok(pagamento);
            });

            app.MapPost("/pagamentos", async (AppDbContext db, Pagamento pagamento) =>
            {
                db.Pagamentos.Add(pagamento);
                await db.SaveChangesAsync();
                return Results.Created($"/pagamentos/{pagamento.Id}", pagamento);
            });
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
            });
            app.MapDelete("/pagamentos/{id}", async (AppDbContext db, int id) =>
            {
                var pagamento = await db.Pagamentos.FindAsync(id);

                if (pagamento is null)
                    return Results.NotFound($"Pagamento com o ID {id} não encontrado");

                db.Pagamentos.Remove(pagamento);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapGet("/pagamentos/{id}", async (AppDbContext db, int id) =>
            {
                var pagamentos = await db.Pagamentos.FindAsync(id);

                if (pagamentos is null)
                    return Results.NotFound($"Pagamento com o ID {id} não encontrados");

                return Results.Ok(pagamentos);
            });
        }
    }
}