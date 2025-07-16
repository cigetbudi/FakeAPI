using Dapper;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities.Common;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure.Persistence.Repositories;

public class DotaVoiceLineRepository : IDotaVoiceLineRepository
{
    private readonly DbFakeApiConnectionFactory _dbConnectionFactory;
    private readonly ITracer _tracer;
    private readonly ILogger<DotaVoiceLineRepository> _logger;

    public DotaVoiceLineRepository(DbFakeApiConnectionFactory dbConnectionFactory, ITracer tracer, ILogger<DotaVoiceLineRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _tracer = tracer;
        _logger = logger;
    }

    public async Task<List<DotaVoiceline>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var activity = _tracer.StartActivity("DotaVoiceLineRepository:GetAllAsync");

        const string sql = """
            SELECT 
                id,
                voice_line AS VoiceLine,
                hero_name AS HeroName
            FROM
                dota_voice_lines;
        """;
        activity?.SetTag("query", sql);

        try
        {
            await using var connection = _dbConnectionFactory.CreateConnectionDBFakeAPI();
            var result = await connection.QueryAsync<DotaVoiceline>(new CommandDefinition(
                sql,
                cancellationToken: cancellationToken
            ));

            return [.. result];
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error in GetAllAsync");
            throw;
        }
    }

    public async Task<List<DotaVoiceline>> GetAllLimitedAsync(int limit, CancellationToken cancellationToken)
    {
        using var activity = _tracer.StartActivity("DotaVoiceLineRepository:GetAllLimitedAsync");

        const string sql = """
            SELECT 
                id,
                voice_line AS VoiceLine,
                hero_name AS HeroName
            FROM
                dota_voice_lines
            ORDER BY RANDOM()
            LIMIT @Limit;
        """;
        activity?.SetTag("query", sql);

        try
        {
            await using var connection = _dbConnectionFactory.CreateConnectionDBFakeAPI();
            var result = await connection.QueryAsync<DotaVoiceline>(
            new CommandDefinition(sql, new { Limit = limit }, cancellationToken: cancellationToken)
        );

            return [.. result];
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error in GetAllLimitedAsync");
            throw;
        }
    }

    public async Task<DotaVoiceline> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var activity = _tracer.StartActivity("DotaVoiceLineRepository:GetByIdAsync");
        const string sql = """
            SELECT 
                id,
                voice_line AS VoiceLine,
                hero_name AS HeroName
            FROM
                dota_voice_lines
            WHERE id = @Id
        """;
        activity?.SetTag("query", sql);

        try
        {
            await using var connection = _dbConnectionFactory.CreateConnectionDBFakeAPI();
            var result = await connection.QueryFirstOrDefaultAsync<DotaVoiceline>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken)
            );

            if (result != null)
            {
                return result;
            }

            var except = new Exception("not found");
            _logger.LogError(except, "Error in GetByIdAsync");
            throw new Exception(except.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync");
            throw;
        }

    }

    public async Task<DotaVoiceline> GetRandomAsync(CancellationToken cancellationToken)
    {
        using var activity = _tracer.StartActivity("DotaVoiceLineRepository:GetRandomAsync");
        const string sql = """
            SELECT 
                id,
                voice_line AS VoiceLine,
                hero_name AS HeroName
            FROM
                dota_voice_lines
            ORDER BY RANDOM()
            LIMIT 1;
        """;
        activity?.SetTag("query", sql);


        try
        {
            await using var connection = _dbConnectionFactory.CreateConnectionDBFakeAPI();
            var result = await connection.QueryFirstOrDefaultAsync<DotaVoiceline>(
                new CommandDefinition(sql, cancellationToken: cancellationToken)
            );

            if (result != null)
            {
                return result;
            }
            
            var except = new Exception("not found");
            _logger.LogError(except, "Error in GetRandomAsync");
            throw new Exception(except.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetRandomAsync");
            throw;
        }        
    }
}
