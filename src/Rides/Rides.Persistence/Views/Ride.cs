namespace Rides.Persistence.Views;

public sealed class Ride : ViewBase<Domain.Model.Ride>
{
    public string RideId { get; set; }

    public string Status { get; set; }

    public string ClientId { get; set; }

    public string CarId { get; set; }

    public DateTimeOffset CreatedTime { get; set; }

    public DateTimeOffset? StartedTime { get; set; }

    public DateTimeOffset? FinishedTime { get; set; }

    public float? OdometerReading { get; set; }

    public string? CancellationReason { get; set; }
    
    public override Domain.Model.Ride ConvertToModel()
    {
        return new Domain.Model.Ride
        {
            RideId = RideId,
            ClientId = ClientId,
            CarId = CarId,
            Status = Status,
            CreatedTime = CreatedTime,
            StartedTime = StartedTime,
            FinishedTime = FinishedTime,
            OdometerReading = OdometerReading,
            CancellationReason = CancellationReason
        };
    }
}