using MessagePack;

namespace Core.Messaging.Events;

[Union(Consts.MessageNumbers.CarCreated, typeof(CarEvents.V1.CarCreated))]
[Union(Consts.MessageNumbers.CarHeld, typeof(CarEvents.V1.CarHeld))]
[Union(Consts.MessageNumbers.CarFreed, typeof(CarEvents.V1.CarFreed))]
public interface IMessage
{
    string Key { get; }
}