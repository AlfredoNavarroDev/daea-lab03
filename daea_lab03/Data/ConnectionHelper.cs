using System.Configuration;

namespace daea_lab03.Data;

public static class ConnectionHelper
{
    public static string DatabaseConnectionString =>
        ConfigurationManager.ConnectionStrings["ReservasDB"].ConnectionString;
}
