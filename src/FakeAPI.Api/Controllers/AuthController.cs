using FakeAPI.Application.Internal.Auth.Commands;
using FakeAPI.Application.Internal.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api.Controllers;

[ApiController]
[Route("api/")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand(request);
        var result = await _mediator.Send(command);

        if (result?.Data?.Token == null)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}
