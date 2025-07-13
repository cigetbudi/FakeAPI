namespace FakeAPI.Application.External.ProductApi.DTOs;

public class ProductApiResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}

