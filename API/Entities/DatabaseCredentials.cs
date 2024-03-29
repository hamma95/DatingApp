namespace API.Entities;

public class DatabaseCredentials
{
    public string DataSource { get; set; }


    public string GetConnectionString()
    {
        return $"Data source={DataSource}";
    }
}