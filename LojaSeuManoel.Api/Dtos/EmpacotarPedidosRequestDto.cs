namespace LojaSeuManoel.Api.Dtos
{
    public class EmpacotarPedidosRequestDto
    {
        public List<PedidoDto> Pedidos { get; set; } = new();
    }

    public class PedidoDto
    {
        public List<ProdutoDto> Produtos { get; set; } = new();
    }
}
