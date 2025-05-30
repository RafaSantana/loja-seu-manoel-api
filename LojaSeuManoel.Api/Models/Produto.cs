namespace LojaSeuManoel.Api.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int? PedidoCaixaId { get; set; }
        public PedidoCaixa? PedidoCaixa { get; set; }
    }
}
