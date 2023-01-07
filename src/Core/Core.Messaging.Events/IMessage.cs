using MessagePack;

namespace Core.Messaging.Events;

[Union(Consts.MessageNumbers.CarCreated, typeof(CarEvents.V1.CarCreated))]
[Union(Consts.MessageNumbers.CarHeld, typeof(CarEvents.V1.CarHeld))]
[Union(Consts.MessageNumbers.CarFreed, typeof(CarEvents.V1.CarFreed))]
[Union(Consts.MessageNumbers.RideCreated, typeof(RideEvents.V1.RideCreated))]
[Union(Consts.MessageNumbers.RideStarted, typeof(RideEvents.V1.RideStarted))]
[Union(Consts.MessageNumbers.RideFinished, typeof(RideEvents.V1.RideFinished))]
[Union(Consts.MessageNumbers.RideCancelled, typeof(RideEvents.V1.RideCancelled))]
[Union(Consts.MessageNumbers.ClientCreated, typeof(ClientEvents.V1.ClientCreated))]
public interface IMessage
{
    string Key { get; }
}