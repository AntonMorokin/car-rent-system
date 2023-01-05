using Rides.Domain.Events;

namespace Rides.Persistence.Views;

public sealed class Car : ViewBase<Domain.Model.Car>
{
    public string CarId { get; set; }

    public string Status { get; set; }
    
    public override Domain.Model.Car ConvertToModel()
    {
        return new Domain.Model.Car(CarId, Status);
    }

    public override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case CarEvents.V1.CarCreated created:
                CarId = created.CarId;
                Status = created.Status.ToString();
                break;
            case CarEvents.V1.CarHeld held:
                Status = held.Status.ToString();
                break;
            case CarEvents.V1.CarFreed freed:
                Status = freed.Status.ToString();
                break;
            default:
                throw new NotSupportedException(
                    $"The view {nameof(Car)} can't process event {evt?.GetType().FullName}");
        }
    }
}