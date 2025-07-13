using FakeAPI.Domain.Entities.Common;

namespace FakeAPI.Application.Common.Interfaces;

public interface IExternalApiLoggingRepository
{
    Task InsertExternalApiLog(CancellationToken cancellationToken, ExternalApiLog log);
}
