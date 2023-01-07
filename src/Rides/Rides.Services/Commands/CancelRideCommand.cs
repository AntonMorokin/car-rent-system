using MediatR;

namespace Rides.Services.Commands;

public sealed class CancelRideCommand : IRequest
{
    public string RideId { get; set; }

    public DateTimeOffset CancelledTime { get; set; }

    public string Reason { get; set; }
}