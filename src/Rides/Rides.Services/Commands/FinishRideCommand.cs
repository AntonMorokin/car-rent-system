using MediatR;

namespace Rides.Services.Commands;

public sealed class FinishRideCommand : IRequest
{
    public string RideId { get; set; }

    public DateTimeOffset FinishedTime { get; set; }

    public float OdometerReading { get; set; }
}