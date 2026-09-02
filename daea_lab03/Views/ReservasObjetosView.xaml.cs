using System.Windows.Controls;
using daea_lab03.ViewModels;

namespace daea_lab03.Views;

public partial class ReservasObjetosView : UserControl
{
    public ReservasObjetosView()
    {
        InitializeComponent();
        DataContext = new ReservasObjetosViewModel();
    }
}
