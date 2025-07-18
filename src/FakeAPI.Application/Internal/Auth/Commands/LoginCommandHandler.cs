using FakeAPI.Application.Common.Interfaces;
using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.Auth.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FakeAPI.Application.Internal.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto?>>
    {
        private readonly IJWTService _jWTService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IJWTService jWTService, ILogger<LoginCommandHandler> logger)
        {
            _jWTService = jWTService;
            _logger = logger;
        }

        public Task<ApiResponse<LoginResponseDto?>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var username = request.LoginRequestDto.Username;
                var password = request.LoginRequestDto.Password;

                if (_jWTService.ValidateUser(username, password))
                {
                    var token = _jWTService.GenerateToken(username);
                    _logger.LogInformation("Token generated for user: {Username}", username);

                    return Task.FromResult(ApiResponse<LoginResponseDto?>.SuccessOK("Login successful", new LoginResponseDto
                    {
                        Token = token
                    }));
                }

                return Task.FromResult(ApiResponse<LoginResponseDto?>.Unauthorized("Invalid username or password"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed due to an unexpected error");
                return Task.FromResult(ApiResponse<LoginResponseDto?>.Unauthorized("Internal error during login"));
            }
        }
    }
}
