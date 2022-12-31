using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Rides.Persistence.Events;

internal sealed class AggregateVersion
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string AggregateName { get; set; }

    public string AggregateId { get; set; }

    public string Version { get; set; }
}