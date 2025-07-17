using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public record GetRandomVoiceLineQuery() : IRequest<ApiResponse<GetVoiceLineResponseDto>>;

