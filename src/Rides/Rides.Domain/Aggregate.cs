using Rides.Domain.Events;

namespace Rides.Domain;

public abstract class Aggregate
{
    private readonly List<(string Version, DomainEventBase Event)> _changes = new(4);

    public IReadOnlyCollection<(string Version, DomainEventBase Event)> Changes => _changes.AsReadOnly();

    private int _numericVersion = -1;
    private int _numericInitialVersion = -1;

    public string Version => _numericVersion.ToString();

    public string InitialVersion => _numericInitialVersion.ToString();

    public abstract string Id { get; }

    protected abstract void When(DomainEventBase evt);

    protected void Apply(DomainEventBase evt)
    {
        When(evt);
        _numericVersion++;
        _changes.Add((Version, evt));
    }

    public void Load(IEnumerable<DomainEventBase> events)
    {
        foreach (var evt in events)
        {
            When(evt);
            _numericVersion++;
        }

        _numericInitialVersion = _numericVersion;
    }
}