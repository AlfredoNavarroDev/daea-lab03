using daea_lab03.Data;
using daea_lab03.Models;
using daea_lab03.MVVM;

namespace daea_lab03.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly UsuarioRepository _repositorio = new();

    private string _username = string.Empty;
    private string _mensajeError = string.Empty;

    public string Username
    {
        get => _username;
        set => SetField(ref _username, value);
    }

    public string MensajeError
    {
        get => _mensajeError;
        set => SetField(ref _mensajeError, value);
    }

    public Usuario? UsuarioAutenticado { get; private set; }

    public bool Ingresar(string password)
    {
        try
        {
            var usuario = _repositorio.ValidarCredenciales(Username.Trim(), password);
            if (usuario is null)
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                return false;
            }

            UsuarioAutenticado = usuario;
            MensajeError = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al validar credenciales: {ex.Message}";
            return false;
        }
    }
}
