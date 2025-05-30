using System;
using System.Collections.Generic;

namespace LojaSeuManoel.Api.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
        public ICollection<PedidoCaixa> CaixasUsadas { get; set; } = new List<PedidoCaixa>();
    }
}
