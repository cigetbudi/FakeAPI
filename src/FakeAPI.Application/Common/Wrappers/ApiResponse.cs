namespace FakeAPI.Application.Common.Wrappers;

public class ApiResponse<T>
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public T? Data { get; set; }

    public ApiResponse(string responseCode, string responseMessage, T? data = default)
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
        Data = data;
    }

    public static ApiResponse<T> SuccessCreated(string message, T data) =>
    new("201", message, data);

    public static ApiResponse<T> SuccessOK(string message, T data) =>
    new("200", message, data);

    public static ApiResponse<T> Fail(string message, string code = "500") =>
    new(code, message, default);

    public static ApiResponse<T> FailNotFound(string message, string code = "404") =>
    new(code, message, default);

    public static ApiResponse<T> FailBadRequest(string message) =>
    new("400", message, default);
    
    public static ApiResponse<T> Unauthorized(string message) =>
    new("401", message, default);
}

