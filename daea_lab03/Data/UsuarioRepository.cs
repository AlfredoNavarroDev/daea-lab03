using daea_lab03.Models;
using Microsoft.Data.SqlClient;

namespace daea_lab03.Data;

/// <summary>
/// Acceso a datos en modo CONECTADO: valida credenciales abriendo la
/// conexión y leyendo el resultado con SqlDataReader.
/// </summary>
public class UsuarioRepository
{
    public Usuario? ValidarCredenciales(string username, string password)
    {
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT UsuarioId, Username, NombreCompleto FROM Usuarios WHERE Username = @Username AND Password = @Password";
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@Password", password);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Usuario
        {
            UsuarioId = reader.GetInt32(0),
            Username = reader.GetString(1),
            NombreCompleto = reader.GetString(2)
        };
    }
}
