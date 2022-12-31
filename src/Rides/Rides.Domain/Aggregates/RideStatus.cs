namespace Rides.Domain.Aggregates;

public enum RideStatus
{
    Unknown,
    Created,
    InProgress,
    Finished,
    Canceled
}