using MessagePack;

namespace Core.Messaging.Events;

[Union(Consts.MessageNumbers.CarCreated, typeof(CarEvents.V1.CarCreated))]
[Union(Consts.MessageNumbers.CarHeld, typeof(CarEvents.V1.CarHeld))]
[Union(Consts.MessageNumbers.CarFreed, typeof(CarEvents.V1.CarFreed))]
[Union(Consts.MessageNumbers.RideCreated, typeof(RideEvents.V1.RideCreated))]
public interface IMessage
{
    string Key { get; }
}