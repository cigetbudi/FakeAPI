using AutoMapper;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.External.ProductApi.DTOs;
using FakeAPI.Application.Internal.Products.DTOs;
using FakeAPI.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Application.Internal.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<CreateProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IExternalProductService _externalProductService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IProductRepository productRepository, IExternalProductService externalProductService, IMapper mapper, ILogger<CreateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _externalProductService = externalProductService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<CreateProductResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map Req dari Controller API ke req External API 
                var externalRequest = _mapper.Map<ProductApiRequestDto>(request.CreateProductData);

                // Hit API External
                var externalResponse = await _externalProductService.CreateProductAsync(externalRequest, cancellationToken);

                if (externalResponse == null || string.IsNullOrEmpty(externalResponse.Id))
                {
                    _logger.LogError("Failed to create product via external service. Response was null or empty.");

                    // return pake pembungkus response
                    return ApiResponse<CreateProductResponseDto>.Fail("Failed to get a valid response from the external service.");
                }

                // Map response eksternal ke entitas domain kita, save ke DB
                var productToSave = _mapper.Map<Product>(externalResponse);
                await _productRepository.AddAsync(productToSave, cancellationToken);

                // Map response eksternal ke DTO response API kita
                var responseData = _mapper.Map<CreateProductResponseDto>(externalResponse);

                // Return response sukses
                _logger.LogInformation("Product {ProductId} - {ProductName} created successfully.", responseData.Id, responseData.Name);
                return ApiResponse<CreateProductResponseDto>.SuccessCreated("Product created successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product.");
                return ApiResponse<CreateProductResponseDto>.Fail("An error occurred while creating the product.");
            }
        }
    }
}