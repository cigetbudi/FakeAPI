
using System.Net.Http.Json;
using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.External.ProductApi.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure.Services;

public class ExternalProductService : IExternalProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _externalApiUrl;
    // private readonly ITracer _tracer;
    private readonly ILogger<ExternalProductService> _logger;

    public ExternalProductService(IHttpClientFactory httpClientFactory, IConfiguration configuration,  ILogger<ExternalProductService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ProductApi");
        _externalApiUrl = configuration["ExternalApiService:Url"]
            ?? throw new InvalidOperationException("External API URL not configured.");
        // _tracer = tracer;
        _logger = logger;
    }

    public async Task<ProductApiResponseDto?> CreateProductAsync(ProductApiRequestDto request, CancellationToken cancellationToken)
    {
        // using var activity = _tracer.StartActivity("ExternalAPI:CreateProduct");
        // activity?.SetTag("http.url", _externalApiUrl);
        // activity?.SetTag("http.method", "POST");

        try
        {
            var response = await _httpClient.PostAsJsonAsync(_externalApiUrl, request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductApiResponseDto>(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

}