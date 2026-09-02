using System.Windows.Controls;
using daea_lab03.ViewModels;

namespace daea_lab03.Views;

public partial class AulasDataTableView : UserControl
{
    public AulasDataTableView()
    {
        InitializeComponent();
        DataContext = new AulasDataTableViewModel();
    }
}
