using System.Data;
using daea_lab03.Data;
using daea_lab03.MVVM;

namespace daea_lab03.ViewModels;

public class ReservasDataTableViewModel : ViewModelBase
{
    private readonly ReservaRepository _repositorio = new();
    private DataTable _tabla = new();
    private string _mensajeEstado = string.Empty;

    public ReservasDataTableViewModel()
    {
        CargarCommand = new RelayCommand(_ => Cargar());
        Cargar();
    }

    public DataView Reservas => _tabla.DefaultView;

    public string MensajeEstado
    {
        get => _mensajeEstado;
        private set => SetField(ref _mensajeEstado, value);
    }

    public RelayCommand CargarCommand { get; }

    private void Cargar()
    {
        try
        {
            _tabla = _repositorio.ObtenerTabla();
            OnPropertyChanged(nameof(Reservas));
            MensajeEstado = $"Reservas cargadas: {_tabla.Rows.Count}.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al cargar reservas: {ex.Message}";
        }
    }
}
