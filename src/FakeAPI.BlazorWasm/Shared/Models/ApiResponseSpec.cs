namespace FakeAPI.BlazorWasm.Shared.Models;

public class ApiResponseSpec
{
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class ApiResponseSpec<T>
{
    public string ResponseCode { get; set; } = default!;
    public string ResponseMessage { get; set; } = default!;
    public T Data { get; set; } = default!;
}