namespace Rides.Persistence.Model;

internal sealed class EventMetadata
{
    public string EventType { get; set; }

    public string AggregateId { get; set; }

    public string AggregateVersion { get; set; }
}