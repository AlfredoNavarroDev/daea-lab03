using System.Data;
using daea_lab03.Models;
using Microsoft.Data.SqlClient;

namespace daea_lab03.Data;

public class ReservaRepository
{
    private const string SelectSql = """
        SELECT r.ReservaId, r.AulaId, r.UsuarioId, r.Fecha, r.Hora, r.Motivo,
               a.Nombre AS AulaNombre, u.NombreCompleto AS UsuarioNombre
        FROM Reservas r
        INNER JOIN Aulas a ON a.AulaId = r.AulaId
        INNER JOIN Usuarios u ON u.UsuarioId = r.UsuarioId
        """;

    /// <summary>
    /// Modo DESCONECTADO: SqlDataAdapter.Fill abre y cierra la conexión
    /// internamente.
    /// </summary>
    public DataTable ObtenerTabla()
    {
        var tabla = new DataTable("Reservas");
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        using var adapter = new SqlDataAdapter(SelectSql, connection);
        adapter.Fill(tabla);
        return tabla;
    }

    /// <summary>
    /// Modo CONECTADO: conexión abierta explícitamente, se recorre el
    /// SqlDataReader fila por fila construyendo cada objeto Reserva.
    /// </summary>
    public List<Reserva> ObtenerLista()
    {
        var reservas = new List<Reserva>();
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SelectSql;

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            reservas.Add(LeerReserva(reader));
        }

        return reservas;
    }

    public List<Reserva> BuscarPorFecha(DateTime fecha)
    {
        var reservas = new List<Reserva>();
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = SelectSql + " WHERE r.Fecha = @Fecha";
        command.Parameters.AddWithValue("@Fecha", fecha.Date);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            reservas.Add(LeerReserva(reader));
        }

        return reservas;
    }

    public void Registrar(Reserva reserva)
    {
        using var connection = new SqlConnection(ConnectionHelper.DatabaseConnectionString);
        connection.Open();

        using (var checkCommand = connection.CreateCommand())
        {
            checkCommand.CommandText = "SELECT COUNT(*) FROM Reservas WHERE AulaId = @AulaId AND Fecha = @Fecha AND Hora = @Hora";
            checkCommand.Parameters.AddWithValue("@AulaId", reserva.AulaId);
            checkCommand.Parameters.AddWithValue("@Fecha", reserva.Fecha.Date);
            checkCommand.Parameters.AddWithValue("@Hora", reserva.Hora);

            var count = (int)checkCommand.ExecuteScalar();
            if (count > 0)
            {
                throw new InvalidOperationException("Ya existe una reserva para esa aula, fecha y hora.");
            }
        }

        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo)
            VALUES (@AulaId, @UsuarioId, @Fecha, @Hora, @Motivo)
            """;
        insertCommand.Parameters.AddWithValue("@AulaId", reserva.AulaId);
        insertCommand.Parameters.AddWithValue("@UsuarioId", reserva.UsuarioId);
        insertCommand.Parameters.AddWithValue("@Fecha", reserva.Fecha.Date);
        insertCommand.Parameters.AddWithValue("@Hora", reserva.Hora);
        insertCommand.Parameters.AddWithValue("@Motivo", reserva.Motivo);

        insertCommand.ExecuteNonQuery();
    }

    private static Reserva LeerReserva(SqlDataReader reader)
    {
        return new Reserva
        {
            ReservaId = reader.GetInt32(0),
            AulaId = reader.GetInt32(1),
            UsuarioId = reader.GetInt32(2),
            Fecha = reader.GetDateTime(3),
            Hora = reader.GetTimeSpan(4),
            Motivo = reader.GetString(5),
            AulaNombre = reader.GetString(6),
            UsuarioNombre = reader.GetString(7)
        };
    }
}
