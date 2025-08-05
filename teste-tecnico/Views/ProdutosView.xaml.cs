using System.Windows.Controls;
using teste_tecnico.ViewModels;

namespace teste_tecnico.Views
{
    /// <summary>
    /// Intera��o l�gica para ProdutosView.xaml
    /// </summary>
    public partial class ProdutosView : UserControl
    {
        public ProdutosView()
        {
            InitializeComponent();
            DataContext = new ProdutosViewModel();
        }
    }
}