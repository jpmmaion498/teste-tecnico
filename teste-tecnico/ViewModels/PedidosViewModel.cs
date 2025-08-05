using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using teste_tecnico.Models;
using teste_tecnico.Services;

namespace teste_tecnico.ViewModels
{
    public class PedidosViewModel : ViewModelBase
    {
        private readonly PedidoService _pedidoService;
        private readonly PessoaService _pessoaService;
        private readonly ProdutoService _produtoService;

        private Pedido _novoPedido;
        private Pessoa _selectedPessoa;
        private Produto _selectedProduto;
        private int _quantidadeProduto = 1;
 
        public ObservableCollection<Pessoa> Pessoas => _pessoaService.Pessoas;
        public ObservableCollection<Produto> Produtos => _produtoService.Produtos;
        public List<FormaPagamento> FormasPagamento { get; }

        public Pedido NovoPedido
        {
            get => _novoPedido;
            set { _novoPedido = value; OnPropertyChanged(); }
        }

        public Pessoa SelectedPessoa
        {
            get => _selectedPessoa;
            set { _selectedPessoa = value; OnPropertyChanged(); }
        }

        public Produto SelectedProduto
        {
            get => _selectedProduto;
            set { _selectedProduto = value; OnPropertyChanged(); }
        }

        public int QuantidadeProduto
        {
            get => _quantidadeProduto;
            set { _quantidadeProduto = value; OnPropertyChanged(); }
        }

        public ICommand AddProdutoCommand { get; }
        public ICommand FinalizarPedidoCommand { get; }

        public PedidosViewModel()
        {
            _pedidoService = PedidoService.Instance;
            _pessoaService = PessoaService.Instance;
            _produtoService = ProdutoService.Instance;

            FormasPagamento = Enum.GetValues(typeof(FormaPagamento)).Cast<FormaPagamento>().ToList();

            NovoPedido = new Pedido();

            AddProdutoCommand = new RelayCommand(AdicionarProduto, CanAdicionarProduto);
            FinalizarPedidoCommand = new RelayCommand(FinalizarPedido, CanFinalizarPedido);
        }

        private void AdicionarProduto(object obj)
        {
            var pedidoItem = new PedidoItem
            {
                ProdutoId = SelectedProduto.Id,
                NomeProduto = SelectedProduto.Nome,
                ValorUnitario = SelectedProduto.Valor,
                Quantidade = QuantidadeProduto
            };

            NovoPedido.Itens.Add(pedidoItem);
            
            OnPropertyChanged(nameof(NovoPedido)); 
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanAdicionarProduto(object obj)
        {
            return SelectedProduto != null && QuantidadeProduto > 0;
        }

        private void FinalizarPedido(object obj)
        {
            NovoPedido.PessoaId = SelectedPessoa.Id;
            NovoPedido.DataVenda = DateTime.Now;
            NovoPedido.Status = StatusPedido.Pendente;

            _pedidoService.Add(NovoPedido);
            _pedidoService.SaveChanges();


            // Reseta para um novo pedido
            NovoPedido = new Pedido();
            SelectedPessoa = null;
            SelectedProduto = null;
            QuantidadeProduto = 1;
        }

        private bool CanFinalizarPedido(object obj)
        {
            return SelectedPessoa != null && NovoPedido.Itens.Any();
        }
    }
}
