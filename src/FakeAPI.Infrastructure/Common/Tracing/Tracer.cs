using System.Diagnostics;
using Microsoft.Extensions.Options;
using FakeAPI.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure.Common.Tracing;

public class Tracer : ITracer
{
    private readonly ActivitySource _activitySource;

    public Tracer(IOptions<TracingOptions> options)
    {
        _activitySource = new ActivitySource(options.Value.ServiceName);
    }

    public Activity? StartActivity(string name, ActivityKind kind = ActivityKind.Internal)
    {
        return _activitySource.StartActivity(name, kind);
    }

    public void SetError(Exception ex, ILogger? logger = null)
    {
        var activity = Activity.Current;
        if (activity != null)
        {
            activity.SetStatus(ActivityStatusCode.Error);
            activity.AddException(ex);
        }

        logger?.LogError(ex, "Error occurred during traced operation: {Message}", ex.Message);
    }
}
