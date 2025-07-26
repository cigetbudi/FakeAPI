using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace FakeAPI.Infrastructure.Common.Auth;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;

    public BasicAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        TimeProvider timeProvider,
        IConfiguration configuration)
        : base(options, loggerFactory, encoder, new SystemClockAdapter(timeProvider))
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var value))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(value);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header format"));

            var username = credentials[0];
            var password = credentials[1];

            var configUsername = _configuration["BasicAuthSettings:Username"];
            var configPassword = _configuration["BasicAuthSettings:Password"];

            if (username != configUsername || password != configPassword)
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}

public class SystemClockAdapter : ISystemClock
{
    private readonly TimeProvider _timeProvider;

    public SystemClockAdapter(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTimeOffset UtcNow => _timeProvider.GetUtcNow();
}
