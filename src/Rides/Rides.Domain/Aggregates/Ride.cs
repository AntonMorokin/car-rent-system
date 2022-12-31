using Rides.Domain.Events;
using Rides.Domain.Exceptions;

namespace Rides.Domain.Aggregates;

public class Ride : Aggregate
{
    private string _id;
    
    public string ClientId { get; private set; }
    public string CarId { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }
    public DateTimeOffset? StartedTime { get; private set; }
    public DateTimeOffset? FinishedTime { get; private set; }
    public float? OdometerReading { get; private set; }
    public string? CancellationReason { get; private set; }
    public RideStatus Status { get; private set; }

    public override string Id => _id;

    public static Ride Create(string id, string clientId, string carId, DateTimeOffset createdTime)
    {
        var inst = new Ride();
        var evt = new RideEvents.V1.RideCreated
        {
            RideId = id,
            ClientId = clientId,
            CarId = carId,
            CreatedTime = createdTime
        };

        inst.Apply(evt);

        return inst;
    }

    public void Start(DateTimeOffset startedTime)
    {
        if (Status != RideStatus.Created)
        {
            throw new DomainException(
                $"Ride can't be started when in current status: {Status.ToString()}");
        }

        if (startedTime < CreatedTime)
        {
            throw new DomainException("Ride start time can't be less than creation time");
        }

        Apply(new RideEvents.V1.RideStarted
        {
            StartedTime = startedTime
        });
    }

    public void Finish(DateTimeOffset finishTime, float odometerReading)
    {
        if (Status != RideStatus.InProgress)
        {
            throw new DomainException("Ride can't be finished when it's not started");
        }

        if (finishTime < StartedTime)
        {
            throw new DomainException("Ride finish time can't be less than start time");
        }

        if (odometerReading < 0)
        {
            throw new DomainException("Current odometer reading must be a positive value");
        }

        Apply(new RideEvents.V1.RideFinished
        {
            FinishedTime = finishTime,
            OdometerReading = odometerReading
        });
    }

    public void Cancel(DateTimeOffset cancelledTime, string reason)
    {
        if (Status != RideStatus.InProgress)
        {
            throw new DomainException("Ride can't be cancelled when it's not started");
        }

        if (cancelledTime < StartedTime)
        {
            throw new DomainException("Ride cancelled time can't be less than start time");
        }

        if (string.IsNullOrEmpty(reason))
        {
            throw new DomainException("Ride cancellation reason can't be empty");
        }
        
        Apply(new RideEvents.V1.RideCancelled
        {
            CancelledTime = cancelledTime,
            Reason = reason
        });
    }

    protected override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case RideEvents.V1.RideCreated rideCreated:
                _id = rideCreated.RideId;
                ClientId = rideCreated.ClientId;
                CarId = rideCreated.CarId;
                CreatedTime = rideCreated.CreatedTime;
                Status = RideStatus.Created;
                break;
            case RideEvents.V1.RideStarted rideStarted:
                StartedTime = rideStarted.StartedTime;
                Status = RideStatus.InProgress;
                break;
            case RideEvents.V1.RideFinished rideFinished:
                FinishedTime = rideFinished.FinishedTime;
                OdometerReading = rideFinished.OdometerReading;
                Status = RideStatus.Finished;
                break;
            case RideEvents.V1.RideCancelled rideCancelled:
                FinishedTime = rideCancelled.CancelledTime;
                CancellationReason = rideCancelled.Reason;
                Status = RideStatus.Canceled;
                break;
            default:
                throw new NotSupportedException(
                    $"Event of type {evt.GetType().FullName} is not supported by {nameof(Ride)} aggregate.");
        }
    }
}