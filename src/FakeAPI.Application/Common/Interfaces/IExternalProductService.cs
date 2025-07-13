using FakeAPI.Application.External.ProductApi.DTOs;

namespace FakeAPI.Application.Common.Interfaces;

public interface IExternalProductService
{
    Task<ProductApiResponseDto?> CreateProductAsync(ProductApiRequestDto request, CancellationToken cancellationToken);
}
