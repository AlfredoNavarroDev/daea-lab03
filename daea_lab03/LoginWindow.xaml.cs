using System.Windows;
using daea_lab03.ViewModels;

namespace daea_lab03;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _viewModel = new();

    public LoginWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void Ingresar_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.Ingresar(PasswordInput.Password))
        {
            var mainWindow = new MainWindow(_viewModel.UsuarioAutenticado!);
            mainWindow.Show();
            Close();
        }
    }
}
