using System.Data;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities.Common;
using Dapper;

namespace FakeAPI.Infrastructure.Persistence.Repositories
{
    public class LoggingRepository(DbConnectionFactory dbConnectionFactory) : ILoggingRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task InsertInterfaceLog(CancellationToken cancellationToken, InterfaceLog log)
        {
            const string sql = @"
            INSERT INTO interface_logs
            (trace_id, service_name, client_name, request_payload, response_payload, request_date, response_date)
            VALUES
            (@TraceID, @ServiceName, @ClientName, @RequestPayload, @ResponsePayload, @RequestDate, @ResponseDate);";

            await using var connection = _dbConnectionFactory.CreateConnectionDBCoba();
            await connection.ExecuteAsync(new CommandDefinition(sql, log, cancellationToken: cancellationToken));
        }
    }
}