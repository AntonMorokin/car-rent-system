using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Rides.Persistence.Views;

public abstract class ReadModelBase
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string AggregateId { get; set; }

    public string Version { get; set; }
}