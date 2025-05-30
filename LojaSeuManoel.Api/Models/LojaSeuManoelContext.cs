using Microsoft.EntityFrameworkCore;

namespace LojaSeuManoel.Api.Models
{
    public class LojaSeuManoelContext : DbContext
    {
        public LojaSeuManoelContext(DbContextOptions<LojaSeuManoelContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Caixa> Caixas { get; set; }
        public DbSet<PedidoCaixa> PedidosCaixa { get; set; }
    }
}
