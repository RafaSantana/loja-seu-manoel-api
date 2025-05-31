namespace LojaSeuManoel.Api.Dtos
{
    public class EmpacotarPedidoRequestDto
    {
        public List<ProdutoDto> Produtos { get; set; } = new();
    }

    public class ProdutoDto
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
    }
}
