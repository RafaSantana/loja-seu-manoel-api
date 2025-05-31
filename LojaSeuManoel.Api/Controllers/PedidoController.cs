using LojaSeuManoel.Api.Dtos;
using LojaSeuManoel.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaSeuManoel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly LojaSeuManoelContext _context;
        public PedidoController(LojaSeuManoelContext context)
        {
            _context = context;
        }

        [HttpPost("empacotar")]
        public async Task<ActionResult<EmpacotarPedidosResponseDto>> EmpacotarPedidos([FromBody] EmpacotarPedidosRequestDto request)
        {
            var caixas = await _context.Caixas.OrderBy(c => c.Comprimento * c.Largura * c.Altura).ToListAsync();
            if (!caixas.Any())
                return BadRequest("Nenhuma caixa cadastrada no sistema.");

            var resposta = new EmpacotarPedidosResponseDto();

            foreach (var pedidoDto in request.Pedidos)
            {
                // Cria o pedido
                var pedido = new Pedido { Data = DateTime.Now };
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                var produtos = pedidoDto.Produtos.Select(p => new Produto
                {
                    Nome = p.Nome,
                    Altura = p.Altura,
                    Largura = p.Largura,
                    Comprimento = p.Comprimento,
                    PedidoId = pedido.Id
                }).ToList();
                _context.Produtos.AddRange(produtos);
                await _context.SaveChangesAsync();

                // Lógica de empacotamento (simples: um produto por caixa menor possível)
                var caixasUsadas = new List<PedidoCaixa>();
                foreach (var produto in produtos)
                {
                    var caixa = caixas.FirstOrDefault(c =>
                        produto.Altura <= c.Altura &&
                        produto.Largura <= c.Largura &&
                        produto.Comprimento <= c.Comprimento);
                    if (caixa == null)
                        return BadRequest($"Produto '{produto.Nome}' não cabe em nenhuma caixa cadastrada.");

                    var pedidoCaixa = caixasUsadas.FirstOrDefault(pc => pc.CaixaId == caixa.Id);
                    if (pedidoCaixa == null)
                    {
                        pedidoCaixa = new PedidoCaixa { PedidoId = pedido.Id, CaixaId = caixa.Id };
                        caixasUsadas.Add(pedidoCaixa);
                    }
                    pedidoCaixa.Produtos.Add(produto);
                    produto.PedidoCaixa = pedidoCaixa;
                }
                _context.PedidosCaixa.AddRange(caixasUsadas);
                await _context.SaveChangesAsync();

                resposta.Pedidos.Add(new PedidoEmpacotadoDto
                {
                    PedidoId = pedido.Id,
                    Caixas = caixasUsadas.Select(pc => new CaixaEmpacotadaDto
                    {
                        NomeCaixa = caixas.First(c => c.Id == pc.CaixaId).Nome,
                        Produtos = pc.Produtos.Select(p => p.Nome).ToList()
                    }).ToList()
                });
            }

            return Ok(resposta);
        }
    }
}
