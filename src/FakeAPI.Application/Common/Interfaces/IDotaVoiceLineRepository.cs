using FakeAPI.Domain.Entities;

namespace FakeAPI.Application.Common.Interfaces;

    public interface IDotaVoiceLineRepository
{
    Task<List<DotaVoiceline>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<DotaVoiceline>> GetAllLimitedAsync(int limit, CancellationToken cancellationToken);
    Task<DotaVoiceline> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<DotaVoiceline> GetRandomAsync(CancellationToken cancellationToken);
}
