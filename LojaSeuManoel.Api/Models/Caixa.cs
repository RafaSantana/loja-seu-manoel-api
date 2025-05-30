namespace LojaSeuManoel.Api.Models
{
    public class Caixa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public ICollection<PedidoCaixa> PedidosCaixa { get; set; } = new List<PedidoCaixa>();
    }
}
