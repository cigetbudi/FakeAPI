namespace FakeAPI.Infrastructure.Common.Tracing;

public class TracingOptions
{
    public string ServiceName { get; set; } = "MyApp";
    public string Exporter { get; set; } = "jaeger";
    public JaegerOptions Jaeger { get; set; } = new();
    public OtlpOptions Otlp { get; set; } = new();
}

public class JaegerOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 6831;
}

public class OtlpOptions
{
    public string Endpoint { get; set; } = "http://localhost:4317";
}
