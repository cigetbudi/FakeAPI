using System.Text.RegularExpressions;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Domain.Entities.Common;

namespace FakeAPI.Api.Middleware;

public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiLoggingMiddleware> _logger;

    public ApiLoggingMiddleware(
        RequestDelegate next,
        ILogger<ApiLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Ambil scoped service dari context
        var tracer = context.RequestServices.GetRequiredService<ITracer>();
        var loggingRepo = context.RequestServices.GetRequiredService<ILoggingRepository>();
        var library = context.RequestServices.GetRequiredService<ILibrary>();

        var traceId = library.GenerateUUID();
        context.Items["TraceID"] = traceId;
        var start = DateTime.UtcNow;

        // Req string
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        requestBody = Regex.Replace(requestBody, @"\r\n?|\n", "");
        context.Request.Body.Position = 0;

        // streaming buat resp
        var originalResponseBody = context.Response.Body;
        using var responseBodyMemory = new MemoryStream();
        context.Response.Body = responseBodyMemory;

        var span = tracer.StartActivity("Middleware:CreateAPILog");
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            tracer.SetError(ex, _logger);
            throw;
        }
        finally
        {
            var finish = DateTime.UtcNow;

            // Resp string
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Restore response
            await responseBodyMemory.CopyToAsync(originalResponseBody);

            // Client name
            var clientName = context.Items["ClientName"]?.ToString() ?? "Default";

            // Simpan log pake repo logging
            var log = new InterfaceLog
            {
                TraceID = traceId,
                ServiceName = context.Request.Path,
                ClientName = clientName,
                RequestPayload = requestBody,
                ResponsePayload = responseBodyText,
                RequestDate = start,
                ResponseDate = finish
            };

            try
            {
                await loggingRepo.InsertInterfaceLog(context.RequestAborted, log);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to insert interface log for {TraceID}", traceId);
            }

            // Log muncul pas beres
           _logger.LogInformation("{{ \"traceId\": \"{TraceId}\", \"method\": \"{Method}\", \"path\": \"{Path}\", \"durationMs\": {Duration} }}",
            traceId,
            context.Request.Method,
            context.Request.Path,
            (finish - start).TotalMilliseconds);


            span?.Dispose();
        }
    }
}
