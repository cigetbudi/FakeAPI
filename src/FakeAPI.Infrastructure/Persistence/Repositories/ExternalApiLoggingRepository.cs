using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities.Common;
using Dapper;

namespace FakeAPI.Infrastructure.Persistence.Repositories;

public class ExternalApiLoggingRepository(DbConnectionFactory dbConnectionFactory) : IExternalApiLoggingRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task InsertExternalApiLog(CancellationToken cancellationToken, ExternalApiLog log)
    {
        const string sql = @"
            INSERT INTO external_api_logs (
            trace_id,
            backend_system,
            service_name,
            http_status,
            request_payload,
            response_payload,
            request_date,
            response_date
        ) VALUES (
            @TraceID,
            @BackendSystem,
            @ServiceName,
            @HttpStatus,
            @RequestPayload,
            @ResponsePayload,
            @RequestDate,
            @ResponseDate
        );";
            await using var connection = _dbConnectionFactory.CreateConnectionDBCoba();
            await connection.ExecuteAsync(new CommandDefinition(sql, log, cancellationToken: cancellationToken));
    }
}
