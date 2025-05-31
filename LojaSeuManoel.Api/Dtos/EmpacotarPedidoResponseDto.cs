namespace LojaSeuManoel.Api.Dtos
{
    public class EmpacotarPedidoResponseDto
    {
        public int PedidoId { get; set; }
        public List<CaixaEmpacotadaDto> Caixas { get; set; } = new();
    }

    public class CaixaEmpacotadaDto
    {
        public string NomeCaixa { get; set; } = string.Empty;
        public List<string> Produtos { get; set; } = new();
    }
}
