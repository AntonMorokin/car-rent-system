using Rides.Domain.Events;
using RideEventsV1 = Rides.Domain.Events.RideEvents.V1;

namespace Rides.Domain.Views;

public sealed class Ride : ViewBase
{
    public string Status { get; set; }

    public string ClientId { get; set; }

    public string CarId { get; set; }

    public DateTimeOffset CreatedTime { get; set; }

    public DateTimeOffset? StartedTime { get; set; }

    public DateTimeOffset? FinishedTime { get; set; }

    public float? OdometerReading { get; set; }

    public string? CancellationReason { get; set; }
    
    public override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case RideEventsV1.RideCreated created:
                AggregateId = created.RideId;
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
            default:
                throw new NotSupportedException(
                    $"The view {nameof(Ride)} can't process event {evt?.GetType().FullName}");
        }
    }
}