namespace FakeAPI.Application.External.ProductApi.DTOs;

public class ProductApiRequestDto
{
    public string Name { get; set; } = string.Empty;
    public ExternalApiDataDto Data { get; set; } = new();
}

public class ExternalApiDataDto
{
    public int Year { get; set; }
    public double Price { get; set; }
    public string CpuModel { get; set; } = string.Empty;
    public string HardDiskSize { get; set; } = string.Empty;
}
