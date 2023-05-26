using System.Configuration;

public static class ConnectionString
{
    public static string WHDBConnection
    {
        get { return ConfigurationManager.ConnectionStrings["WHDBConnection"].ConnectionString; }
    }
}
