using System.Windows.Controls;
using daea_lab03.ViewModels;

namespace daea_lab03.Views;

public partial class NuevaReservaView : UserControl
{
    public NuevaReservaView(int usuarioId)
    {
        InitializeComponent();
        DataContext = new NuevaReservaViewModel(usuarioId);
    }
}
