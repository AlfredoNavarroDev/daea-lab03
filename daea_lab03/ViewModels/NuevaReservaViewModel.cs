using System.Collections.ObjectModel;
using System.Linq;
using daea_lab03.Data;
using daea_lab03.Models;
using daea_lab03.MVVM;

namespace daea_lab03.ViewModels;

public class NuevaReservaViewModel : ViewModelBase
{
    private readonly ReservaRepository _reservaRepositorio = new();
    private readonly AulaRepository _aulaRepositorio = new();

    private Aula? _aulaSeleccionada;
    private DateTime? _fecha = DateTime.Today;
    private TimeSpan? _horaSeleccionada;
    private string _motivo = string.Empty;
    private string _mensajeEstado = string.Empty;

    public NuevaReservaViewModel(int usuarioId)
    {
        UsuarioId = usuarioId;
        GuardarCommand = new RelayCommand(_ => Guardar());
        CargarAulas();
    }

    public int UsuarioId { get; }

    public ObservableCollection<Aula> Aulas { get; } = [];

    /// <summary>
    /// Franjas horarias disponibles cada 30 minutos, de 07:00 a 21:00.
    /// </summary>
    public List<TimeSpan> HorasDisponibles { get; } =
        Enumerable.Range(0, 29)
            .Select(i => TimeSpan.FromMinutes(7 * 60 + i * 30))
            .ToList();

    public Aula? AulaSeleccionada
    {
        get => _aulaSeleccionada;
        set => SetField(ref _aulaSeleccionada, value);
    }

    public DateTime? Fecha
    {
        get => _fecha;
        set => SetField(ref _fecha, value);
    }

    public TimeSpan? HoraSeleccionada
    {
        get => _horaSeleccionada;
        set => SetField(ref _horaSeleccionada, value);
    }

    public string Motivo
    {
        get => _motivo;
        set => SetField(ref _motivo, value);
    }

    public string MensajeEstado
    {
        get => _mensajeEstado;
        private set => SetField(ref _mensajeEstado, value);
    }

    public RelayCommand GuardarCommand { get; }

    private void CargarAulas()
    {
        try
        {
            Aulas.Clear();
            foreach (var aula in _aulaRepositorio.ObtenerLista())
            {
                Aulas.Add(aula);
            }
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al cargar aulas: {ex.Message}";
        }
    }

    private void Guardar()
    {
        if (AulaSeleccionada is null)
        {
            MensajeEstado = "Selecciona un aula.";
            return;
        }

        if (Fecha is null)
        {
            MensajeEstado = "Selecciona una fecha.";
            return;
        }

        if (HoraSeleccionada is null)
        {
            MensajeEstado = "Selecciona una hora.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Motivo))
        {
            MensajeEstado = "Ingresa un motivo.";
            return;
        }

        try
        {
            var reserva = new Reserva
            {
                AulaId = AulaSeleccionada.AulaId,
                UsuarioId = UsuarioId,
                Fecha = Fecha.Value,
                Hora = HoraSeleccionada.Value,
                Motivo = Motivo.Trim()
            };

            _reservaRepositorio.Registrar(reserva);

            MensajeEstado = "Reserva registrada correctamente.";
            HoraSeleccionada = null;
            Motivo = string.Empty;
        }
        catch (InvalidOperationException ex)
        {
            MensajeEstado = ex.Message;
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al registrar la reserva: {ex.Message}";
        }
    }
}
