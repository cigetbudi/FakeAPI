using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Infrastructure.Common.ExternalApiLogging;
using FakeAPI.Infrastructure.Common.Library;
using FakeAPI.Infrastructure.Common.Tracing;
using FakeAPI.Infrastructure.Persistence;
using FakeAPI.Infrastructure.Persistence.Repositories;
using FakeAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Koneksi db
        services.AddSingleton<DbConnectionFactory>();
        services.AddSingleton<DbFakeApiConnectionFactory>();

        // Repo Product (urusan ke DB)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IDotaVoiceLineRepository, DotaVoiceLineRepository>();
        // Repo Logging (ke db juga)
        services.AddScoped<ILoggingRepository, LoggingRepository>();
        services.AddScoped<IExternalApiLoggingRepository, ExternalApiLoggingRepository>();

        // Service Product (urusan ke External Api)
        services.AddScoped<IExternalProductService, ExternalProductService>();

        // Library
        // Tracer
        services.AddScoped<ITracer, Tracer>();
        services.AddTransient<HttpClientTracingHandler>();

        // DAFTARKAN NAMED HTTP CLIENTS DENGAN CUSTOM HANDLER
        services.AddHttpClient("ProductApi")
            .AddHttpMessageHandler(sp =>
            {
                // Membuat instance ExternalApiLoggingHandler dengan ClientName "DefaultExternalApi"
                var logger = sp.GetRequiredService<ILogger<ExternalApiLoggingHandler>>();
                var library = sp.GetRequiredService<ILibrary>();
                var exloggingRepo = sp.GetRequiredService<IExternalApiLoggingRepository>();
                return new ExternalApiLoggingHandler(logger, library, exloggingRepo, "ProductApiService");
            })
            .AddHttpMessageHandler<HttpClientTracingHandler>(); 

        services.AddScoped<ILibrary, Library>();

        return services;
    }
}