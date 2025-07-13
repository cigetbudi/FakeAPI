using System.Text;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities.Common;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure.Common.ExternalApiLogging;

public class ExternalApiLoggingHandler : DelegatingHandler
{
    private readonly ILogger<ExternalApiLoggingHandler> _logger;
    private readonly ILibrary _library;
    private readonly IExternalApiLoggingRepository _exloggingRepository;
    private readonly string _clientName; // Namanya kudu sama pas mau dipake logging di DI

    public ExternalApiLoggingHandler(
        ILogger<ExternalApiLoggingHandler> logger,
        ILibrary library,
        IExternalApiLoggingRepository exloggingRepository,
        string clientName)
    {
        _logger = logger;
        _library = library;
        _exloggingRepository = exloggingRepository;
        _clientName = clientName; 
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var traceId = _library.GenerateUUID();
        var start = DateTime.UtcNow;

        var curl = new StringBuilder();
        curl.Append("curl");
        curl.Append($" -X {request.Method} \"{request.RequestUri}\"");

        foreach (var header in request.Headers)
            curl.Append($" -H \"{header.Key}: {string.Join(", ", header.Value)}\"");

        string? requestBody = null;
        if (request.Content != null)
        {
            requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            curl.Append($" -d '{requestBody}'");
        }

        var response = await base.SendAsync(request, cancellationToken);

        string responseBody = "";
        if (response.Content != null)
        {
            responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var finish = DateTime.UtcNow;

        var log = new ExternalApiLog
        {
            TraceID = traceId,
            BackendSystem = request.RequestUri?.AbsoluteUri ?? "-",
            ServiceName = _clientName,
            HttpStatus = (int)response.StatusCode,
            RequestPayload = curl.ToString(),
            ResponsePayload = responseBody,
            RequestDate = start,
            ResponseDate = finish
        };

        try
        {
            await _exloggingRepository.InsertExternalApiLog(cancellationToken, log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to insert external API log for {TraceID}", traceId);
        }

        return response;
    }
}