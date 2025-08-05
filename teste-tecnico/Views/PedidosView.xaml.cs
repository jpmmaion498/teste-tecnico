using System.Windows.Controls;
using teste_tecnico.ViewModels;

namespace teste_tecnico.Views
{
    /// <summary>
    /// Intera��o l�gica para PedidosView.xaml
    /// </summary>
    public partial class PedidosView : UserControl
    {
        public PedidosView()
        {
            InitializeComponent();
            DataContext = new PedidosViewModel();
        }
    }
}