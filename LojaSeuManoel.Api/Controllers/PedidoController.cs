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
            // Busca as caixas ordenadas do menor para o maior volume
            var caixas = await _context.Caixas.OrderBy(c => c.Comprimento * c.Largura * c.Altura).ToListAsync();
            if (!caixas.Any())
                return BadRequest("Nenhuma caixa cadastrada no sistema.");

            var resposta = new EmpacotarPedidosResponseDto();

            foreach (var pedidoDto in request.Pedidos)
            {
                // Cria o pedido no banco
                var pedido = new Pedido { Data = DateTime.Now };
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Cria os produtos do pedido
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

                // Algoritmo First Fit Decreasing simplificado para empacotamento
                // 1. Ordena os produtos do maior para o menor volume
                var produtosRestantes = produtos
                    .OrderByDescending(p => p.Altura * p.Largura * p.Comprimento)
                    .ToList();

                var caixasUsadas = new List<(Caixa caixa, List<Produto> produtos)>();

                while (produtosRestantes.Any())
                {
                    bool produtoEmpacotado = false;
                    // Tenta cada caixa, da menor para a maior
                    foreach (var caixa in caixas)
                    {
                        // Tenta encaixar o máximo de produtos possível nesta caixa
                        var produtosParaEstaCaixa = new List<Produto>();

                        // Espaço "restante" simplificado: só aceita produtos que cabem individualmente
                        // (não faz empacotamento 3D real, mas agrupa produtos pequenos juntos)
                        foreach (var produto in produtosRestantes.ToList())
                        {
                            if (produto.Altura <= caixa.Altura &&
                                produto.Largura <= caixa.Largura &&
                                produto.Comprimento <= caixa.Comprimento)
                            {
                                produtosParaEstaCaixa.Add(produto);
                            }
                        }

                        if (produtosParaEstaCaixa.Any())
                        {
                            // Remove os produtos empacotados da lista de produtos restantes
                            foreach (var p in produtosParaEstaCaixa)
                                produtosRestantes.Remove(p);

                            caixasUsadas.Add((caixa, produtosParaEstaCaixa));
                            produtoEmpacotado = true;
                            break; // Volta para o while para empacotar o restante
                        }
                    }
                    if (!produtoEmpacotado)
                    {
                        // Se algum produto não couber em nenhuma caixa, retorna erro
                        var produto = produtosRestantes.First();
                        return BadRequest($"Produto '{produto.Nome}' não cabe em nenhuma caixa cadastrada.");
                    }
                }

                // Salva as caixas usadas e os produtos empacotados no banco
                var pedidoCaixas = new List<PedidoCaixa>();
                foreach (var (caixa, produtosNaCaixa) in caixasUsadas)
                {
                    var pedidoCaixa = new PedidoCaixa
                    {
                        PedidoId = pedido.Id,
                        CaixaId = caixa.Id,
                        Produtos = produtosNaCaixa
                    };
                    pedidoCaixas.Add(pedidoCaixa);
                }
                _context.PedidosCaixa.AddRange(pedidoCaixas);
                await _context.SaveChangesAsync();

                // Monta a resposta para o pedido
                resposta.Pedidos.Add(new PedidoEmpacotadoDto
                {
                    PedidoId = pedido.Id,
                    Caixas = pedidoCaixas.Select(pc => new CaixaEmpacotadaDto
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
