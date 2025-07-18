using FakeAPI.Application.Common.Wrappers;
using FakeAPI.Application.Internal.Auth.DTOs;
using MediatR;

namespace FakeAPI.Application.Internal.Auth.Commands;

public class LoginCommand(LoginRequestDto loginRequestDto) : IRequest<ApiResponse<LoginResponseDto?>>
{
    public LoginRequestDto LoginRequestDto { get; set; } = loginRequestDto;
}
