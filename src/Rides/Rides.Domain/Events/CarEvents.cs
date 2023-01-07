using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rides.Domain.Aggregates;

namespace Rides.Domain.Events;

public static class CarEvents
{
    public static class V1
    {
        public record CarCreated : DomainEventBase
        {
            public string CarId { get; set; }

            [BsonRepresentation(BsonType.String)]
            public CarStatus Status { get; set; }
        }

        public record CarHeld : DomainEventBase 
        {
            public string RideId { get; set; }

            [BsonRepresentation(BsonType.String)]
            public CarStatus Status { get; set; }
        }

        public record CarFreed : DomainEventBase
        {
            [BsonRepresentation(BsonType.String)]
            public CarStatus Status { get; set; }
        }
    }
}