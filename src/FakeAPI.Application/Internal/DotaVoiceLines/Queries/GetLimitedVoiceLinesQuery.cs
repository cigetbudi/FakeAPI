using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public class GetLimitedVoiceLinesQuery(int limit) : IRequest<ApiResponse<List<GetVoiceLineResponseDto>>>
{
    public int Limit { get; set; } = limit;
}
