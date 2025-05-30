using System.Collections.Generic;

namespace LojaSeuManoel.Api.Models
{
    public class PedidoCaixa
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int CaixaId { get; set; }
        public Caixa? Caixa { get; set; }
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
