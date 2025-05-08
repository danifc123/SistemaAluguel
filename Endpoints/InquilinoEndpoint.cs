using System.Security;
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
            // Validação de campos obrigatórios
            if (string.IsNullOrWhiteSpace(inquilino.Nome) ||
                string.IsNullOrWhiteSpace(inquilino.CPF) ||
                string.IsNullOrWhiteSpace(inquilino.Email) ||
                string.IsNullOrWhiteSpace(inquilino.Telefone))
                {
                    return Results.BadRequest("Todos os campos são obrigatórios.");
                }
            //Validando o email
            if (!inquilino.Email.Contains("@"))
                return Results.BadRequest("Email inválido.");
            //Verificando o tamanho do CPF
            if (inquilino.CPF.Length != 11)
                return Results.BadRequest("CPF inválido. Deve conter 11 dígitos.");

               // Verifica se o e-mail já está cadastrado
            var emailExistente = await db.Inquilinos.AnyAsync(i => i.Email == inquilino.Email);
            if (emailExistente)
                return Results.BadRequest("E-mail já cadastrado.");

               // Verifica se o CPF já está cadastrado
            var cpfExistente = await db.Inquilinos.AnyAsync(i => i.CPF == inquilino.CPF);
            if (cpfExistente)
                return Results.BadRequest("CPF já cadastrado.");

            db.Inquilinos.Add(inquilino);
            await db.SaveChangesAsync();

            return Results.Created($"/inquilinos/{inquilino.Id}", inquilino);

            }).RequireAuthorization();


            app.MapPut("/inquilinos", async (AppDbContext db, Inquilino inquilino) =>
            {
                var inquilinoExistente = await db.Inquilinos.FindAsync(inquilino.Id);
                if (inquilinoExistente is null)
                    return Results.NotFound($"Inquilino com ID {inquilino.Id} não encontrado.");

                // Validação de campos obrigatórios
                if (string.IsNullOrWhiteSpace(inquilino.Nome) ||
                    string.IsNullOrWhiteSpace(inquilino.Email) ||
                    string.IsNullOrWhiteSpace(inquilino.CPF) ||
                    string.IsNullOrWhiteSpace(inquilino.Telefone))
                {
                    return Results.BadRequest("Todos os campos são obrigatórios.");
                }

                if (!inquilino.Email.Contains("@"))
                    return Results.BadRequest("Email inválido.");

                if (inquilino.CPF.Length != 11)
                    return Results.BadRequest("CPF inválido. Deve conter 11 dígitos.");

                // Verifica se o e-mail já está em uso por outro inquilino
                var emailEmUso = await db.Inquilinos
                    .AnyAsync(i => i.Email == inquilino.Email && i.Id != inquilino.Id);
                if (emailEmUso)
                    return Results.BadRequest("E-mail já está em uso por outro inquilino.");

                // Verifica se o CPF já está em uso por outro inquilino
                var cpfEmUso = await db.Inquilinos
                    .AnyAsync(i => i.CPF == inquilino.CPF && i.Id != inquilino.Id);
                if (cpfEmUso)
                    return Results.BadRequest("CPF já está em uso por outro inquilino.");

                // Atualização dos dados
                inquilinoExistente.Nome = inquilino.Nome;
                inquilinoExistente.Email = inquilino.Email;
                inquilinoExistente.CPF = inquilino.CPF;
                inquilinoExistente.Telefone = inquilino.Telefone;

                await db.SaveChangesAsync();

                return Results.Ok(inquilinoExistente);

            }).RequireAuthorization();

            app.MapDelete("/inquilinos/{id}", async (AppDbContext db, int id) =>
            {
                var possuiContratoAtivo = await db.Contratos.AnyAsync(c => c.InquilinoId == id && c.Ativo);

                if (possuiContratoAtivo)
                    return Results.BadRequest("Este inquilino possui contratos ativos e não pode ser excluído.");

                var inquilino = await db.Inquilinos.FindAsync(id);

                if (inquilino is null)
                    return Results.NotFound($"Inquilino com ID {id} não encontrado.");

                db.Inquilinos.Remove(inquilino);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapGet("/inquilinos/{id}", async (AppDbContext db, int id) =>
            {
                if(id <= 0)
                return Results.BadRequest("ID invalido");

                var inquilino = await db.Inquilinos
                .Where(i => i.Id == id)
                .Select(i => new InquilinoDTO
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Email = i.Email,
                    Telefone = i.Telefone,
                    CPF = i.CPF
                })
                .FirstOrDefaultAsync();

                if (inquilino is null)
                    return Results.NotFound($"ID {id} não encontrado");

                return Results.Ok(inquilino);
            });
            app.MapGet("/inquilinos", async (AppDbContext db, string? nome) =>
            {
                var query = db.Inquilinos.AsQueryable();

                var nomeLimpo = nome?.Trim().ToLower();

                if (!string.IsNullOrWhiteSpace(nomeLimpo))
                {
                    query = query.Where(i => i.Nome!.ToLower().Contains(nomeLimpo));
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
