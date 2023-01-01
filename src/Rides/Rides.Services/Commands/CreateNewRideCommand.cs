using MediatR;

namespace Rides.Services.Commands;

public sealed class CreateNewRideCommand : IRequest
{
    public string RideId { get; set; }

    public string ClientId { get; set; }

    public string CarId { get; set; }

    public DateTimeOffset CreatedTime { get; set; }
}