using System.Windows.Controls;
using teste_tecnico.ViewModels;

namespace teste_tecnico.Views
{
    /// <summary>
    /// Interação lógica para PessoasView.xaml
    /// </summary>
    public partial class PessoasView : UserControl
    {
        public PessoasView()
        {
            InitializeComponent();
            DataContext = new PessoasViewModel();
        }
    }
}
