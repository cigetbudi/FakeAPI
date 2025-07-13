using FakeAPI.Application.Internal.Products.Commands;
using FakeAPI.Application.Internal.Products.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
    {
        var command = new CreateProductCommand(request);
        var result = await _mediator.Send(command);

        if (result.ResponseCode != "201")
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return CreatedAtAction(null, result.Data, result);
    }
}
