using FakeAPI.Application.Internal.DotaVoiceLines.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "BasicAuth")]
public class DotaVoiceLinesBasicController : ControllerBase
{
    private readonly ISender _mediator;

    public DotaVoiceLinesBasicController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllVoiceLines()
    {
        var query = new GetAllVoiceLinesQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("random")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRandomVoiceLine()
    {
        var query = new GetRandomVoiceLineQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("limit/{limit}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLimit([FromRoute] int limit)
    {
        var query = new GetLimitedVoiceLinesQuery(limit);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetVoiceLineByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
