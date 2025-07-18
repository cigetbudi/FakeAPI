using System.Text;
using FakeAPI.Api.Middleware;
using FakeAPI.Application;
using FakeAPI.Infrastructure;
using FakeAPI.Infrastructure.Common.Tracing;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using FluentValidation.AspNetCore;
using FluentValidation;
using App = FakeAPI.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting up...");

    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5121);
        
    });

    

    builder.Host.UseSerilog(); // Pasang setelah builder

    // Service Registrations
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();

    // Tracing options
    builder.Services.Configure<TracingOptions>(
        builder.Configuration.GetSection("Tracing"));

    var tracingOptions = builder.Configuration
        .GetSection("Tracing")
        .Get<TracingOptions>();

    // Konfigur OpenTelemetry sama Jaeger
    builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        var serviceName = tracingOptions?.ServiceName ?? "MyService";

        t.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation(options =>
        {
            var requestBodyKey = new HttpRequestOptionsKey<string>("http.request.body");
            var responseBodyKey = new HttpRequestOptionsKey<string>("http.response.body");

            options.EnrichWithHttpRequestMessage = (activity, request) =>
            {
                activity.SetTag("http.method", request.Method.ToString());
                activity.SetTag("http.url", request.RequestUri?.ToString());

                foreach (var header in request.Headers)
                {
                    activity.SetTag($"http.request.header.{header.Key}", string.Join(",", header.Value));
                }

                // Body
                if (request.Options.TryGetValue(requestBodyKey, out var body))
                {
                    activity.SetTag("http.request.body", body);
                }

                // Curl representation
                var curl = new StringBuilder();
                curl.Append("curl");
                curl.Append($" -X {request.Method}");
                curl.Append($" \"{request.RequestUri}\"");

                foreach (var header in request.Headers)
                {
                    curl.Append($" -H \"{header.Key}: {string.Join(",", header.Value)}\"");
                }

                if (!string.IsNullOrWhiteSpace(body))
                {
                    curl.Append($" -d '{body}'");
                }

                activity.SetTag("http.request.curl", curl.ToString());
            };

            options.EnrichWithHttpResponseMessage = (activity, response) =>
            {
                activity.SetTag("http.status_code", (int)response.StatusCode);

                var req = response.RequestMessage;
                var key = new HttpRequestOptionsKey<string>("http.response.body");

                if (req?.Options.TryGetValue(key, out var body) == true)
                {
                    activity.SetTag("http.response.body", body);
                }
            };

        })
        .AddSource(serviceName);

        var exporter = tracingOptions?.Exporter?.ToLowerInvariant();

        if (exporter == "jaeger")
        {
            t.AddJaegerExporter(options =>
            {
                options.AgentHost = tracingOptions?.Jaeger.Host ?? "localhost";
                options.AgentPort = tracingOptions?.Jaeger.Port ?? 8888;
            });
        }
        else if (exporter == "otlp")
        {
            t.AddOtlpExporter(options =>
            {
                var endpoint = tracingOptions?.Otlp.Endpoint ?? "http://localhost:4317";
                options.Endpoint = new Uri(endpoint);
            });
        }
    });

    // Bearer token JWT
    builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidAudience = config["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
        };
    });

    builder.Services.AddAuthorization();

    // FluentValidation
    builder.Services.AddValidatorsFromAssembly(typeof(App.DependencyInjection).Assembly);
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();

    // API
    builder.Services.AddControllers();

    builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "FakeAPI.Api", Version = "v1" });

        // 🔒 Tambahkan ini
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Input your bearer token from /api/login. Contoh: Bearer eyJhbGciOiJIUzI1NiIs..."
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });


    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseRouting();
    app.UseMiddleware<ApiLoggingMiddleware>();
    app.UseMiddleware<ValidationExceptionMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}
