using AutoMapper;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public class GetAllVoiceLinesQueryHandler 
    : IRequestHandler<GetAllVoiceLinesQuery, ApiResponse<List<GetVoiceLineResponseDto>>>
{
    private readonly IDotaVoiceLineRepository _dotaVoiceLineRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllVoiceLinesQueryHandler> _logger;

    public GetAllVoiceLinesQueryHandler(IDotaVoiceLineRepository dotaVoiceLineRepository, IMapper mapper, ILogger<GetAllVoiceLinesQueryHandler> logger)
    {
        _dotaVoiceLineRepository = dotaVoiceLineRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<List<GetVoiceLineResponseDto>>> Handle(GetAllVoiceLinesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _dotaVoiceLineRepository.GetAllAsync(cancellationToken);
            var responseData = _mapper.Map<List<GetVoiceLineResponseDto>>(data);

            _logger.LogInformation("Voice Lines successfully retrieved");
            return ApiResponse<List<GetVoiceLineResponseDto>>.SuccessOK("Success", responseData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving Voice Lines.");
            return ApiResponse<List<GetVoiceLineResponseDto>>.Fail("An error occurred while retrieving the voice lines.");
        }
    }
}
