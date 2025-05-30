using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;
using SistemaAluguel.DTOs;
using SistemaAluguel.Models;
using SistemaAluguel.Services;

namespace SistemaAluguelAPI.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuariosEndpoints( this IEndpointRouteBuilder app )
        {
           app.MapPost("/usuarios/register", async (AppDbContext db, UsuarioRegisterDTO dto, PasswordHasher hasher) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Senha))
                    return Results.BadRequest("Todos os campos são obrigatórios.");

                var usuarioExistente = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (usuarioExistente is not null)
                    return Results.BadRequest("Já existe um usuário com esse e-mail.");

                var novoUsuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    SenhaHash = hasher.Hash(dto.Senha)
                };

                db.Usuarios.Add(novoUsuario);
                await db.SaveChangesAsync();

                    return Results.Created($"/usuarios/{novoUsuario.Id}", 
                    new { novoUsuario.Id, novoUsuario.Email });
            });

            app.MapPost("/usuarios/login", async (
                AppDbContext db,
                UsuarioLoginDTO dto,
                PasswordHasher hasher,
                TokenService tokenService) => 
            {
                if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
                    return Results.BadRequest("Email e senha são obrigatórios");

                var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (usuario is null)
                    return Results.Unauthorized();

                var senhaCorreta = hasher.Verify(dto.Senha, usuario.SenhaHash!);
                if (!senhaCorreta)
                    return Results.Unauthorized();

                var token = tokenService.GenerateToken(usuario);
                    return Results.Ok(new { token });
            });
        }
    }
}