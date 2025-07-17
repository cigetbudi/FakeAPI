using AutoMapper;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public class GetRandomVoiceLineQueryHandler : IRequestHandler<GetRandomVoiceLineQuery, ApiResponse<GetVoiceLineResponseDto>>
{
    private readonly IDotaVoiceLineRepository _dotaVoiceLineRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRandomVoiceLineQueryHandler> _logger;

    public GetRandomVoiceLineQueryHandler(IDotaVoiceLineRepository dotaVoiceLineRepository, IMapper mapper, ILogger<GetRandomVoiceLineQueryHandler> logger)
    {
        _dotaVoiceLineRepository = dotaVoiceLineRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ApiResponse<GetVoiceLineResponseDto>> Handle(GetRandomVoiceLineQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _dotaVoiceLineRepository.GetRandomAsync(cancellationToken);
            var responseData = _mapper.Map<GetVoiceLineResponseDto>(data);

            _logger.LogInformation("Random Voice Line successfully retrieved");
            return ApiResponse<GetVoiceLineResponseDto>.SuccessOK("Success", responseData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving Random Voice Line.");
            return ApiResponse<GetVoiceLineResponseDto>.Fail("An error occurred while retrieving the random voice line.");
        }
    }
}
