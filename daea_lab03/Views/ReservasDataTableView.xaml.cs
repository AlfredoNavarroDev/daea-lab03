using System.Windows.Controls;
using daea_lab03.ViewModels;

namespace daea_lab03.Views;

public partial class ReservasDataTableView : UserControl
{
    public ReservasDataTableView()
    {
        InitializeComponent();
        DataContext = new ReservasDataTableViewModel();
    }
}
