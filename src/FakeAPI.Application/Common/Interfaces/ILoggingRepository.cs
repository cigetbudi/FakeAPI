using FakeAPI.Domain.Entities.Common;

namespace FakeAPI.Application.Common.Interfaces;

public interface ILoggingRepository
{
    Task InsertInterfaceLog(CancellationToken cancellationToken, InterfaceLog log);
}

