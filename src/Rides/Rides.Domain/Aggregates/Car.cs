using Rides.Domain.Events;
using Rides.Domain.Exceptions;

namespace Rides.Domain.Aggregates;

public sealed class Car : Aggregate
{
    private string _id;

    public override string Id => _id;

    public string? RideId { get; set; }

    public CarStatus Status { get; set; }

    public static Car Create(string id)
    {
        var inst = new Car();
        var evt = new CarEvents.V1.CarCreated
        {
            CarId = id,
            Status = CarStatus.Ready
        };

        inst.Apply(evt);

        return inst;
    }

    public void StartRide(string rideId)
    {
        if (Status != CarStatus.Ready)
        {
            throw new DomainException(
                $"Car with id={Id} in status {Status.ToString()} can't be assigned to the ride");
        }

        Apply(new CarEvents.V1.CarHeld
        {
            RideId = rideId,
            Status = CarStatus.Busy
        });
    }

    public void FinishRide()
    {
        if (Status != CarStatus.Busy)
        {
            throw new DomainException($"The car with id={Id} in status {Status.ToString()} can't be freed");
        }
        
        Apply(new CarEvents.V1.CarFreed
        {
            Status = CarStatus.Ready
        });
    }

    protected override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case CarEvents.V1.CarCreated created:
                _id = created.CarId;
                Status = created.Status;
                break;
            case CarEvents.V1.CarHeld held:
                RideId = held.RideId;
                Status = held.Status;
                break;
            case CarEvents.V1.CarFreed freed:
                RideId = null;
                Status = freed.Status;
                break;
            default:
                throw new NotSupportedException(
                    $"Event of type {evt.GetType().FullName} is not supported by {nameof(Car)} aggregate.");
        }
    }
}