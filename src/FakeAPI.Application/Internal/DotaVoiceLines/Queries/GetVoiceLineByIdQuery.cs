using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public class GetVoiceLineByIdQuery(int id) : IRequest<ApiResponse<GetVoiceLineResponseDto>>
{
    public int Id { get; set; } = id;
}
