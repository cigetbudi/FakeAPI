using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.Products.DTOs;
using MediatR;

namespace FakeAPI.Application.Internal.Products.Commands;

public class CreateProductCommand(CreateProductRequestDto createProductData) : IRequest<ApiResponse<CreateProductResponseDto>>
{
    public CreateProductRequestDto CreateProductData { get; set; } = createProductData;
}
