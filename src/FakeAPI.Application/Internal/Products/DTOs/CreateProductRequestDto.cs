namespace FakeAPI.Application.Internal.Products.DTOs;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public double Price { get; set; }
    public string CpuModel { get; set; } = string.Empty;
    public string HardDiskSize { get; set; } = string.Empty;
}
