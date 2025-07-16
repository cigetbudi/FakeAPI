using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FakeAPI.Infrastructure.Persistence;

public class DbFakeApiConnectionFactory
{

    private readonly string _connectionStringDBFakeAPI;

    public DbFakeApiConnectionFactory(IConfiguration configuration)
    {
        _connectionStringDBFakeAPI = configuration.GetConnectionString("DBFakeAPI")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
    }
    
    public NpgsqlConnection CreateConnectionDBFakeAPI() => new NpgsqlConnection(_connectionStringDBFakeAPI);
}
