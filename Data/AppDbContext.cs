using Microsoft.EntityFrameworkCore;
using SistemaAluguel.Models;

namespace SistemaAluguel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Imovel> Imoveis { get; set; }  // Isso representa a tabela de imóveis
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Usuario> Usuarios {get; set;}
    }
}
