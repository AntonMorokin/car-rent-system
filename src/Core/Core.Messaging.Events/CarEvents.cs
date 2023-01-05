using MessagePack;

namespace Core.Messaging.Events;

public static class CarEvents
{
    public static class V1
    {
        [MessagePackObject]
        public sealed class CarCreated : IMessage
        {
            [IgnoreMember]
            public string Key => Consts.Prefixes.Car + CarId;

            [Key(0)]
            public string CarId { get; init; }

            [Key(1)]
            public string Number { get; init; }

            [Key(2)]
            public string Brand { get; init; }

            [Key(3)]
            public string Model { get; init; }
        }
        
        [MessagePackObject]
        public sealed class CarHeld : IMessage
        {
            [IgnoreMember]
            public string Key => Consts.Prefixes.Car + CarId;

            [Key(0)]
            public string CarId { get; init; }

            [Key(1)]
            public string RideId { get; set; }
        }
        
        [MessagePackObject]
        public sealed class CarFreed : IMessage
        {
            [IgnoreMember]
            public string Key => Consts.Prefixes.Car + CarId;

            [Key(0)]
            public string CarId { get; set; }
        }
    }
}