using System.Data;
using daea_lab03.Models;
using Microsoft.Data.SqlClient;

namespace daea_lab03.Data;

public class AulaRepository
{
    private const string SelectSql = "SELECT AulaId, Nombre, Capacidad FROM Aulas";

    /// <summary>
    /// Modo DESCONECTADO: SqlDataAdapter.Fill abre y cierra la conexión
    /// internamente; no queda abierta al retornar el DataTable.
    /// </summary>
    public DataTable ObtenerTabla()
    {
        var tabla = new DataTable("Aulas");
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        using var adapter = new SqlDataAdapter(SelectSql, connection);
        adapter.Fill(tabla);
        return tabla;
    }

    /// <summary>
    /// Modo CONECTADO: conexión abierta explícitamente, se recorre el
    /// SqlDataReader fila por fila construyendo cada objeto Aula.
    /// </summary>
    public List<Aula> ObtenerLista()
    {
        var aulas = new List<Aula>();
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SelectSql;

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            aulas.Add(LeerAula(reader));
        }

        return aulas;
    }

    public List<Aula> BuscarPorNombre(string nombre)
    {
        var aulas = new List<Aula>();
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SelectSql + " WHERE Nombre LIKE @Nombre";
        command.Parameters.AddWithValue("@Nombre", $"%{nombre}%");

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            aulas.Add(LeerAula(reader));
        }

        return aulas;
    }

    private static Aula LeerAula(SqlDataReader reader)
    {
        return new Aula
        {
            AulaId = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Capacidad = reader.GetInt32(2)
        };
    }
}
