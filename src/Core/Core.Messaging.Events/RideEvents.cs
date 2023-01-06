using MessagePack;

namespace Core.Messaging.Events;

public static class RideEvents
{
    public static class V1
    {
        [MessagePackObject]
        public sealed class RideCreated : IMessage
        {
            [IgnoreMember]
            public string Key => Consts.Prefixes.Ride + RideId;

            [Key(0)]
            public string RideId { get; set; }

            [Key(1)]
            public string ClientId { get; set; }

            [Key(2)]
            public string CarId { get; set; }

            [Key(3)]
            public DateTimeOffset CreatedTime { get; set; }
        }
    }
}