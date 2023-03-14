namespace WebApi.Net6;

public class AppSettings
{
    public string password { get; set; }
    public string engine { get; set; }
    public int port { get; set; }
    public string dbInstanceIdentifier { get; set; }
    public string host { get; set; }
    public string username { get; set; }

    public string AsConnectionString()
    {
        return $"Data Source={this.host}; Initial Catalog={this.dbInstanceIdentifier}; User Id={this.username}; Password={this.password}";
    }
}