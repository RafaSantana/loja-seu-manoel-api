using Microsoft.EntityFrameworkCore;

namespace LojaSeuManoel.Api.Models
{
    public class LojaSeuManoelContext : DbContext
    {
        private const string DecimalColumnType = "decimal(18,2)";

        public LojaSeuManoelContext(DbContextOptions<LojaSeuManoelContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<PedidoCaixa> PedidosCaixa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Caixa>(entity =>
            {
                entity.Property(e => e.Altura).HasColumnType(DecimalColumnType);
                entity.Property(e => e.Largura).HasColumnType(DecimalColumnType);
                entity.Property(e => e.Comprimento).HasColumnType(DecimalColumnType);
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.Altura).HasColumnType(DecimalColumnType);
                entity.Property(e => e.Largura).HasColumnType(DecimalColumnType);
                entity.Property(e => e.Comprimento).HasColumnType(DecimalColumnType);
            });
        }
    }
}
