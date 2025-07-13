using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FakeAPI.Infrastructure.Persistence;

public class DbConnectionFactory
{

    private readonly string _connectionStringDBCoba;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionStringDBCoba = configuration.GetConnectionString("DBCoba")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");
    }
    
    public NpgsqlConnection CreateConnectionDBCoba() => new NpgsqlConnection(_connectionStringDBCoba);
}
