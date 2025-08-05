using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace teste_tecnico.Models
{
    public enum FormaPagamento
    {
        Dinheiro,
        Cartao,
        Boleto
    }

    public enum StatusPedido
    {
        Pendente,
        Pago,
        Enviado,
        Recebido
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public ObservableCollection<PedidoItem> Itens { get; set; }
        public decimal ValorTotal => Itens?.Sum(i => i.SubTotal) ?? 0;
        public DateTime DataVenda { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public StatusPedido Status { get; set; }

        public Pedido()
        {
            Itens = new ObservableCollection<PedidoItem>();
        }
    }
}
