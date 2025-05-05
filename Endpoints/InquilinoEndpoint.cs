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
        }
    }
}