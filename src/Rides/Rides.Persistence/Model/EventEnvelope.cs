using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Rides.Domain;
using Rides.Domain.Events;

namespace Rides.Persistence.Model;

internal sealed class EventEnvelope
{
    private const string V1 = "v1";

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string Version { get; set; } = V1;

    public EventMetadata Meta { get; set; }

    public DomainEventBase Payload { get; set; }

    public static EventEnvelope FromEvent<T>(Aggregate aggregate, string aggregateVersion, T evt)
        where T : DomainEventBase
    {
        if (evt is null)
        {
            throw new ArgumentNullException(nameof(evt));
        }

        return new EventEnvelope
        {
            Version = V1,
            Meta = new EventMetadata
            {
                AggregateId = aggregate.Id,
                AggregateVersion = aggregateVersion,
                EventType = typeof(T).FullName!
            },
            Payload = evt
        };
    }

    public EventEnvelope()
    {
    }

    public T GetEvent<T>() where T : DomainEventBase => (T)Payload;
}