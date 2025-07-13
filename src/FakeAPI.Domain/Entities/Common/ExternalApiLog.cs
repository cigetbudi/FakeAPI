namespace FakeAPI.Domain.Entities.Common;



public class ExternalApiLog
    {
        public string TraceID { get; set; } = string.Empty;
        public string BackendSystem { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public int HttpStatus { get; set; }
        public string RequestPayload { get; set; } = string.Empty;
        public string ResponsePayload { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.UtcNow;
    }