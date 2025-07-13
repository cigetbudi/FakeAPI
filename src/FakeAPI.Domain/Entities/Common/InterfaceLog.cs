namespace FakeAPI.Domain.Entities.Common;

public class InterfaceLog
{
    public string TraceID { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string RequestPayload { get; set; } = string.Empty;
    public string ResponsePayload { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DateTime ResponseDate { get; set; }
}