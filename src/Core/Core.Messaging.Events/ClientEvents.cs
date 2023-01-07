using MessagePack;

namespace Core.Messaging.Events;

public static class ClientEvents
{
    public static class V1
    {
        [MessagePackObject]
        public sealed class ClientCreated : IMessage
        {
            [IgnoreMember]
            public string Key => Consts.Prefixes.Client + ClientId;

            [Key(0)]
            public string ClientId { get; set; }

            [Key(1)]
            public string FirstName { get; set; }

            [Key(2)]
            public string LastName { get; set; }

            [Key(3)]
            public DateOnly BirthDate { get; set; }
        }
    }
}