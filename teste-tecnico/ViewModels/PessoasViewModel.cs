using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using teste_tecnico.Models;
using teste_tecnico.Services;
using System.Collections.Specialized;

namespace teste_tecnico.ViewModels
{
    public class PessoasViewModel : ViewModelBase
    {
        private readonly PessoaService _pessoaService;
        private readonly PedidoService _pedidoService;
        private Pessoa _selectedPessoa;
        private string _filtroNome;
        private string _filtroCpf;

        private ObservableCollection<Pedido> _pedidosDaPessoa;
        public ICollectionView PessoasView { get; }
        public ICollectionView PedidosDaPessoaView { get; }

        private bool _mostrarPendentes;
        public bool MostrarPendentes { get => _mostrarPendentes; set { _mostrarPendentes = value; OnPropertyChanged(); PedidosDaPessoaView.Refresh(); } }

        private bool _mostrarPagos;
        public bool MostrarPagos { get => _mostrarPagos; set { _mostrarPagos = value; OnPropertyChanged(); PedidosDaPessoaView.Refresh(); } }

        private bool _mostrarEntregues;
        public bool MostrarEntregues { get => _mostrarEntregues; set { _mostrarEntregues = value; OnPropertyChanged(); PedidosDaPessoaView.Refresh(); } }


        public string FiltroNome
        {
            get => _filtroNome;
            set { _filtroNome = value; OnPropertyChanged(); PessoasView.Refresh(); }
        }

        public string FiltroCpf
        {
            get => _filtroCpf;
            set { _filtroCpf = value; OnPropertyChanged(); PessoasView.Refresh(); }
        }

        public Pessoa SelectedPessoa
        {
            get => _selectedPessoa;
            set
            {
                _selectedPessoa = value;
                OnPropertyChanged();
                AtualizarPedidosDaPessoa();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand MarcarComoPagoCommand { get; }
        public ICommand MarcarComoEnviadoCommand { get; }
        public ICommand MarcarComoRecebidoCommand { get; }

        public PessoasViewModel()
        {
            _pessoaService = PessoaService.Instance;
            _pedidoService = PedidoService.Instance;

            PessoasView = CollectionViewSource.GetDefaultView(_pessoaService.Pessoas);
            PessoasView.Filter = FiltroPessoaPredicate;

            _pedidosDaPessoa = new ObservableCollection<Pedido>();
            PedidosDaPessoaView = CollectionViewSource.GetDefaultView(_pedidosDaPessoa);
            PedidosDaPessoaView.Filter = FiltroPedidoPredicate;

            _pedidoService.Pedidos.CollectionChanged += (s, e) => AtualizarPedidosDaPessoa();

            AddCommand = new RelayCommand(AddNewPessoa);
            SaveCommand = new RelayCommand(SaveChanges);
            DeleteCommand = new RelayCommand(DeletePessoa, CanDeletePessoa);
            
            MarcarComoPagoCommand = new RelayCommand(MarcarComoPago);
            MarcarComoEnviadoCommand = new RelayCommand(MarcarComoEnviado);
            MarcarComoRecebidoCommand = new RelayCommand(MarcarComoRecebido);
        }

        private bool FiltroPedidoPredicate(object item)
        {
            if (item is Pedido pedido)
            {
                if (!MostrarPendentes && !MostrarPagos && !MostrarEntregues)
                    return true;

                bool corresponde = false;
                if (MostrarPendentes && pedido.Status == StatusPedido.Pendente) corresponde = true;
                if (MostrarPagos && pedido.Status == StatusPedido.Pago) corresponde = true;
                if (MostrarEntregues && pedido.Status == StatusPedido.Recebido) corresponde = true;

                return corresponde;
            }
            return false;
        }

        private void AtualizarPedidosDaPessoa()
        {
            _pedidosDaPessoa.Clear();
            if (SelectedPessoa != null)
            {
                var pedidos = _pedidoService.Pedidos.Where(p => p.PessoaId == SelectedPessoa.Id);
                foreach (var pedido in pedidos)
                {
                    _pedidosDaPessoa.Add(pedido);
                }
            }
        }

        private bool FiltroPessoaPredicate(object item)
        {
            if (item is Pessoa pessoa)
            {
                bool nomeValido = string.IsNullOrWhiteSpace(FiltroNome) || pessoa.Nome.ToLower().Contains(FiltroNome.ToLower());
                bool cpfValido = string.IsNullOrWhiteSpace(FiltroCpf) || pessoa.CPF.Contains(FiltroCpf);
                return nomeValido && cpfValido;
            }
            return false;
        }

        private void AddNewPessoa(object obj)
        {
            var novaPessoa = new Pessoa { Nome = "Novo Nome", CPF = "000.000.000-00" };
            _pessoaService.Add(novaPessoa);
            PessoasView.MoveCurrentTo(novaPessoa);
        }

        private void SaveChanges(object obj)
        {
            _pessoaService.SaveChanges();
            _pedidoService.SaveChanges();
        }

        private void DeletePessoa(object obj)
        {
            if (SelectedPessoa != null)
            {
                _pessoaService.Delete(SelectedPessoa.Id);
                _pessoaService.SaveChanges();
            }
        }

        private bool CanDeletePessoa(object obj)
        {
            return SelectedPessoa != null;
        }

        private void MarcarComoPago(object pedidoObj)
        {
            if (pedidoObj is Pedido pedido && pedido.Status == StatusPedido.Pendente)
            {
                pedido.Status = StatusPedido.Pago;
                _pedidoService.SaveChanges();
                PedidosDaPessoaView.Refresh();
            }
        }

        private void MarcarComoEnviado(object pedidoObj)
        {
            if (pedidoObj is Pedido pedido && pedido.Status == StatusPedido.Pago)
            {
                pedido.Status = StatusPedido.Enviado;
                _pedidoService.SaveChanges();
                PedidosDaPessoaView.Refresh();
            }
        }

        private void MarcarComoRecebido(object pedidoObj)
        {
            if (pedidoObj is Pedido pedido && pedido.Status == StatusPedido.Enviado)
            {
                pedido.Status = StatusPedido.Recebido;
                _pedidoService.SaveChanges();
                PedidosDaPessoaView.Refresh();
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
