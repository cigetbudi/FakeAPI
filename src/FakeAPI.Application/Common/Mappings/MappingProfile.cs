using AutoMapper;
using FakeAPI.Application.External.ProductApi.DTOs;
using FakeAPI.Application.Internal.Products.DTOs;
using FakeAPI.Domain.Entities;

namespace FakeAPI.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping ke payload data dari API eksternal
        CreateMap<CreateProductRequestDto, ExternalApiDataDto>();

        // Mapping dari Request Controller ke Request API Eksternal
        CreateMap<CreateProductRequestDto, ProductApiRequestDto>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));

        // Mapping dari Response API Eksternal ke Domain 
        CreateMap<ProductApiResponseDto, Product>();

        // Mapping dari Response API Eksternal ke DTO Response
        CreateMap<ProductApiResponseDto, CreateProductResponseDto>();
    }
}

