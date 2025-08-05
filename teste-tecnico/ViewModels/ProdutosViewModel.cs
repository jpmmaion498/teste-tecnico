using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using teste_tecnico.Models;
using teste_tecnico.Services;

namespace teste_tecnico.ViewModels
{
    public class ProdutosViewModel : ViewModelBase
    {
        private readonly ProdutoService _produtoService;
        private Produto _selectedProduto;
        private string _filtroNome;
        private string _filtroCodigo;
        private decimal? _filtroValorInicial;
        private decimal? _filtroValorFinal;

        public ICollectionView ProdutosView { get; }

        public string FiltroNome
        {
            get => _filtroNome;
            set { _filtroNome = value; OnPropertyChanged(); AplicarFiltro(); }
        }

        public string FiltroCodigo
        {
            get => _filtroCodigo;
            set { _filtroCodigo = value; OnPropertyChanged(); AplicarFiltro(); }
        }

        public decimal? FiltroValorInicial
        {
            get => _filtroValorInicial;
            set { _filtroValorInicial = value; OnPropertyChanged(); AplicarFiltro(); }
        }

        public decimal? FiltroValorFinal
        {
            get => _filtroValorFinal;
            set { _filtroValorFinal = value; OnPropertyChanged(); AplicarFiltro(); }
        }

        public Produto SelectedProduto
        {
            get => _selectedProduto;
            set { _selectedProduto = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProdutosViewModel()
        {
            _produtoService = ProdutoService.Instance;
            ProdutosView = CollectionViewSource.GetDefaultView(_produtoService.Produtos);
            ProdutosView.Filter = FiltroPredicate;

            AddCommand = new RelayCommand(AddNewProduto);
            SaveCommand = new RelayCommand(SaveChanges);
            DeleteCommand = new RelayCommand(DeleteProduto, CanDeleteProduto);
        }

        private bool FiltroPredicate(object item)
        {
            if (item is Produto produto)
            {
                bool nomeValido = string.IsNullOrWhiteSpace(FiltroNome) || produto.Nome.ToLower().Contains(FiltroNome.ToLower());
                bool codigoValido = string.IsNullOrWhiteSpace(FiltroCodigo) || produto.Codigo.ToLower().Contains(FiltroCodigo.ToLower());
                bool valorInicialValido = !FiltroValorInicial.HasValue || produto.Valor >= FiltroValorInicial.Value;
                bool valorFinalValido = !FiltroValorFinal.HasValue || produto.Valor <= FiltroValorFinal.Value;

                return nomeValido && codigoValido && valorInicialValido && valorFinalValido;
            }
            return false;
        }

        private void AplicarFiltro()
        {
            ProdutosView.Refresh();
        }

        private void AddNewProduto(object obj)
        {
            var novoProduto = new Produto { Nome = "Novo Produto", Codigo = "COD000", Valor = 0 };
            _produtoService.Add(novoProduto);
            SelectedProduto = novoProduto;
            ProdutosView.MoveCurrentTo(novoProduto);
        }

        private void SaveChanges(object obj)
        {
            _produtoService.SaveChanges();
        }

        private void DeleteProduto(object obj)
        {
            if (SelectedProduto != null)
            {
                _produtoService.Delete(SelectedProduto.Id);
                _produtoService.SaveChanges();
            }
        }

        private bool CanDeleteProduto(object obj)
        {
            return SelectedProduto != null;
        }
    }
}
