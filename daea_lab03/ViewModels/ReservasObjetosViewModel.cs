using System.Collections.ObjectModel;
using daea_lab03.Data;
using daea_lab03.Models;
using daea_lab03.MVVM;

namespace daea_lab03.ViewModels;

public class ReservasObjetosViewModel : ViewModelBase
{
    private readonly ReservaRepository _repositorio = new();
    private DateTime? _fechaBusqueda = DateTime.Today;
    private string _mensajeEstado = string.Empty;

    public ReservasObjetosViewModel()
    {
        CargarCommand = new RelayCommand(_ => Cargar());
        BuscarCommand = new RelayCommand(_ => Buscar());
        Cargar();
    }

    public ObservableCollection<Reserva> Reservas { get; } = [];

    public DateTime? FechaBusqueda
    {
        get => _fechaBusqueda;
        set => SetField(ref _fechaBusqueda, value);
    }

    public string MensajeEstado
    {
        get => _mensajeEstado;
        private set => SetField(ref _mensajeEstado, value);
    }

    public RelayCommand CargarCommand { get; }
    public RelayCommand BuscarCommand { get; }

    private void Cargar()
    {
        try
        {
            Reemplazar(_repositorio.ObtenerLista());
            MensajeEstado = $"Reservas cargadas: {Reservas.Count}.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al cargar reservas: {ex.Message}";
        }
    }

    private void Buscar()
    {
        if (FechaBusqueda is null)
        {
            MensajeEstado = "Selecciona una fecha para buscar.";
            return;
        }

        try
        {
            Reemplazar(_repositorio.BuscarPorFecha(FechaBusqueda.Value));
            MensajeEstado = $"Resultados encontrados: {Reservas.Count}.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al buscar: {ex.Message}";
        }
    }

    private void Reemplazar(List<Reserva> reservas)
    {
        Reservas.Clear();
        foreach (var reserva in reservas)
        {
            Reservas.Add(reserva);
        }
    }
}
