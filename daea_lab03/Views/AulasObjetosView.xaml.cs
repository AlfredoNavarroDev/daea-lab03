using System.Windows.Controls;
using daea_lab03.ViewModels;

namespace daea_lab03.Views;

public partial class AulasObjetosView : UserControl
{
    public AulasObjetosView()
    {
        InitializeComponent();
        DataContext = new AulasObjetosViewModel();
    }
}
