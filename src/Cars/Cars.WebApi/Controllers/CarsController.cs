using System.Threading;
using System.Threading.Tasks;
using Cars.Services;
using Cars.WebApi.Contract.Responses;
using Cars.WebApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cars.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CarsController : ControllerBase
{
    private readonly ICarsService _carsService;

    public CarsController(ICarsService carsService)
    {
        _carsService = carsService;
    }

    [HttpPut]
    public Task<string> CreateNewCarAsync([FromBody] CreateNewCarRequest request,
        CancellationToken cancellationToken)
    {
        return _carsService.CreateNewCarAsync(request.Number,
            request.Brand,
            request.Model,
            request.Mileage,
            cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> GetCarAsync([FromQuery] string? id, [FromQuery] string? number, CancellationToken cancellationToken)
    {
        var noId = string.IsNullOrEmpty(id);
        if (!(noId ^ string.IsNullOrEmpty(number)))
        {
            return BadRequest($"You must specify either {nameof(id)} or {nameof(number)}");
        }

        var car = noId
            ? await _carsService.GetCarByNumberAsync(number!, cancellationToken)
            : await _carsService.GetCarByIdAsync(id!, cancellationToken);

        var response = new CarResponse(car.Id, car.Number, car.Brand, car.Model, car.Mileage);
        return Ok(response);
    }
}