using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities;
using FakeAPI.Infrastructure.Services;
using Dapper;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;
    private readonly ITracer _tracer;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(DbConnectionFactory dbConnectionFactory, ITracer tracer, ILogger<ProductRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _tracer = tracer;
        _logger = logger;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        using var activity = _tracer.StartActivity("ProductRepository:AddAsync");

        const string sql = """
            INSERT INTO products (id, name, created_at)
            VALUES (@Id, @Name, @CreatedAt)
            ON CONFLICT (id) DO NOTHING;
            """;
        activity?.SetTag("query", sql);
        

        try
        {
            await using var connection = _dbConnectionFactory.CreateConnectionDBCoba();
            await connection.ExecuteAsync(
                new CommandDefinition(sql, product, cancellationToken: cancellationToken)
            );
        }
        catch (Exception ex)
        {
            _tracer.SetError(ex, _logger);
            throw;
        }
    }
}
