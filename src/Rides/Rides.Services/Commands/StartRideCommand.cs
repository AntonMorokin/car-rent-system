using MediatR;

namespace Rides.Services.Commands;

public sealed class StartRideCommand : IRequest
{
    public string RideId { get; set; }

    public DateTimeOffset StartedTime { get; set; }
}