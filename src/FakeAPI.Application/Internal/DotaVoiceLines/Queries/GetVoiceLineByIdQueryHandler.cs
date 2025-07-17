using AutoMapper;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.DotaVoiceLines.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Application.Internal.DotaVoiceLines.Queries;

public class GetVoiceLineByIdQueryHandler : IRequestHandler<GetVoiceLineByIdQuery, ApiResponse<GetVoiceLineResponseDto>>
{
    private readonly IDotaVoiceLineRepository _dotaVoiceLineRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVoiceLineByIdQueryHandler> _logger;

    public GetVoiceLineByIdQueryHandler(IDotaVoiceLineRepository dotaVoiceLineRepository, IMapper mapper, ILogger<GetVoiceLineByIdQueryHandler> logger)
    {
        _dotaVoiceLineRepository = dotaVoiceLineRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ApiResponse<GetVoiceLineResponseDto>> Handle(GetVoiceLineByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _dotaVoiceLineRepository.GetByIdAsync(request.Id, cancellationToken);

            if (data == null)
            {
                _logger.LogWarning("Voice Line with ID {Id} not found.", request.Id);
                return ApiResponse<GetVoiceLineResponseDto>.FailNotFound("Voice Line not found");
            }


            var responseData = _mapper.Map<GetVoiceLineResponseDto>(data);

            _logger.LogInformation("Voice Line successfully retrieved");
            return ApiResponse<GetVoiceLineResponseDto>.SuccessOK("Success", responseData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving Voice Line.");
            return ApiResponse<GetVoiceLineResponseDto>.Fail("An error occurred while retrieving the voice line.");

        }
    }
}
