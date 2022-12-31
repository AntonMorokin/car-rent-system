namespace Rides.Domain.Views;

public record Ride
{
    public string RideId { get; init; }

    public string Status { get; init; }

    public string ClientId { get; init; }

    public string CarId { get; init; }

    public DateTimeOffset CreatedTime { get; init; }

    public DateTimeOffset? StartedTime { get; init; }

    public DateTimeOffset? FinishedTime { get; init; }

    public float? OdometerReading { get; init; }

    public string? CancellationReason { get; init; }
}