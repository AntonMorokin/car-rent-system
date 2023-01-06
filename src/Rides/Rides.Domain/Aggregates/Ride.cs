using Rides.Domain.Events;
using Rides.Domain.Exceptions;

namespace Rides.Domain.Aggregates;

public class Ride : Aggregate
{
    private string _id;

    public override string Id => _id;
    public string ClientId { get; private set; }
    public string CarId { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }
    public DateTimeOffset? StartedTime { get; private set; }
    public DateTimeOffset? FinishedTime { get; private set; }
    public float? OdometerReading { get; private set; }
    public string? CancellationReason { get; private set; }
    public RideStatus Status { get; private set; }

    public static Ride Create(string id,
        string clientId,
        Car car,
        DateTimeOffset createdTime)
    {
        if (car.Status != CarStatus.Ready)
        {
            throw new DomainException(
                $"The ride with id={id} can't be started because the car with id={car.Id} isn't ready");
        }

        var inst = new Ride();
        var evt = new RideEvents.V1.RideCreated
        {
            RideId = id,
            ClientId = clientId,
            CarId = car.Id,
            CreatedTime = createdTime,
            Status = RideStatus.Created
        };

        inst.Apply(evt);
        return inst;
    }

    public void Start(Car car, DateTimeOffset startedTime)
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

        if (!(car.Status == CarStatus.Busy
              && car.RideId == Id))
        {
            throw new DomainException(
                $"The car with id={CarId} wasn't held yet for the ride with id={Id}");
        }

        Apply(new RideEvents.V1.RideStarted
        {
            StartedTime = startedTime,
            Status = RideStatus.InProgress
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
            OdometerReading = odometerReading,
            Status = RideStatus.Finished
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
            Reason = reason,
            Status = RideStatus.Canceled
        });
    }

    protected override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case RideEvents.V1.RideCreated created:
                _id = created.RideId;
                ClientId = created.ClientId;
                CarId = created.CarId;
                CreatedTime = created.CreatedTime;
                Status = created.Status;
                break;
            case RideEvents.V1.RideStarted started:
                StartedTime = started.StartedTime;
                Status = RideStatus.InProgress;
                Status = started.Status;
                break;
            case RideEvents.V1.RideFinished finished:
                FinishedTime = finished.FinishedTime;
                OdometerReading = finished.OdometerReading;
                Status = finished.Status;
                break;
            case RideEvents.V1.RideCancelled cancelled:
                FinishedTime = cancelled.CancelledTime;
                CancellationReason = cancelled.Reason;
                Status = cancelled.Status;
                break;
            default:
                throw new NotSupportedException(
                    $"Event of type {evt.GetType().FullName} is not supported by {nameof(Ride)} aggregate.");
        }
    }
}