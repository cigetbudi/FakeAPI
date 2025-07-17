using FakeAPI.Application.Internal.DotaVoiceLines.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DotaVoiceLinesController : ControllerBase
{
    private readonly ISender _mediator;

    public DotaVoiceLinesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("/all")]
    public async Task<IActionResult> GetAllVoiceLines()
    {
        var query = new GetAllVoiceLinesQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("/random")]
    public async Task<IActionResult> GetRandomVoiceLine()
    {
        var query = new GetRandomVoiceLineQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("/limit/{limit}")]
    public async Task<IActionResult> GetAllLimit([FromRoute] int limit)
    {
        var query = new GetLimitedVoiceLinesQuery(limit);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetVoiceLineByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }


}
