namespace LojaSeuManoel.Api.Dtos
{
    public class EmpacotarPedidosResponseDto
    {
        public List<PedidoEmpacotadoDto> Pedidos { get; set; } = new();
    }

    public class PedidoEmpacotadoDto
    {
        public int PedidoId { get; set; }
        public List<CaixaEmpacotadaDto> Caixas { get; set; } = new();
    }
}
