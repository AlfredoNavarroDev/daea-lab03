using System.Collections.ObjectModel;
using daea_lab03.Data;
using daea_lab03.Models;
using daea_lab03.MVVM;

namespace daea_lab03.ViewModels;

public class AulasObjetosViewModel : ViewModelBase
{
    private readonly AulaRepository _repositorio = new();
    private string _nombreBusqueda = string.Empty;
    private string _mensajeEstado = string.Empty;

    public AulasObjetosViewModel()
    {
        CargarCommand = new RelayCommand(_ => Cargar());
        BuscarCommand = new RelayCommand(_ => Buscar());
        Cargar();
    }

    public ObservableCollection<Aula> Aulas { get; } = [];

    public string NombreBusqueda
    {
        get => _nombreBusqueda;
        set => SetField(ref _nombreBusqueda, value);
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
            MensajeEstado = $"Aulas cargadas: {Aulas.Count}.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al cargar aulas: {ex.Message}";
        }
    }

    private void Buscar()
    {
        try
        {
            Reemplazar(_repositorio.BuscarPorNombre(NombreBusqueda.Trim()));
            MensajeEstado = $"Resultados encontrados: {Aulas.Count}.";
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error al buscar: {ex.Message}";
        }
    }

    private void Reemplazar(List<Aula> aulas)
    {
        Aulas.Clear();
        foreach (var aula in aulas)
        {
            Aulas.Add(aula);
        }
    }
}
