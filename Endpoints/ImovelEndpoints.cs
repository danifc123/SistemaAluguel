using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Data;

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

        // Outros endpoints do imóvel...
    }
}
}