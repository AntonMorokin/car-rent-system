using Rides.Domain.Events;
using RideEventsV1 = Rides.Domain.Events.RideEvents.V1;

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

    public override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case RideEventsV1.RideCreated created:
                RideId = created.RideId;
                ClientId = created.ClientId;
                CarId = created.CarId;
                CreatedTime = created.CreatedTime;
                Status = created.Status.ToString();
                break;
            case RideEventsV1.RideStarted started:
                StartedTime = started.StartedTime;
                Status = started.Status.ToString();
                break;
            case RideEventsV1.RideFinished finished:
                FinishedTime = finished.FinishedTime;
                OdometerReading = finished.OdometerReading;
                Status = finished.Status.ToString();
                break;
            case RideEventsV1.RideCancelled cancelled:
                FinishedTime = cancelled.CancelledTime;
                CancellationReason = cancelled.Reason;
                Status = cancelled.Status.ToString();
                break;
        }
    }
}