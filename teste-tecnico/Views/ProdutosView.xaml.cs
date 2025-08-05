using System.Windows.Controls;
using teste_tecnico.ViewModels;

namespace teste_tecnico.Views
{
    /// <summary>
    /// Interação lógica para ProdutosView.xaml
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