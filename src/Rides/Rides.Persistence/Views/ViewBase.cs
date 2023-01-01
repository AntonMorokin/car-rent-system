using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Rides.Domain.Events;

namespace Rides.Persistence.Views;

public abstract class ViewBase
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string AggregateId { get; set; }

    public string Version { get; set; }

    public abstract void When(DomainEventBase evt);
}

public abstract class ViewBase<T> : ViewBase
{
    public abstract T ConvertToModel();
}