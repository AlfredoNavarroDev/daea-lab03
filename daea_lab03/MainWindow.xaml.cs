using System.Windows;
using daea_lab03.Models;
using daea_lab03.Views;

namespace daea_lab03
{
    public partial class MainWindow : Window
    {
        private readonly Usuario _usuario;

        public MainWindow(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            WelcomeText.Text = $"Bienvenido, {_usuario.NombreCompleto}";
        }

        private void AulasDataTable_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentana("Aulas (DataTable)", new AulasDataTableView());
        }

        private void AulasObjetos_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentana("Aulas (Objetos)", new AulasObjetosView());
        }

        private void ReservasDataTable_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentana("Reservas (DataTable)", new ReservasDataTableView());
        }

        private void ReservasObjetos_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentana("Reservas (Objetos)", new ReservasObjetosView());
        }

        private void NuevaReserva_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentana("Nueva reserva", new NuevaReservaView(_usuario.UsuarioId));
        }

        private static void AbrirVentana(string titulo, UIElement contenido)
        {
            new Window
            {
                Title = titulo,
                Content = contenido,
                Width = 700,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }.Show();
        }
    }
}
