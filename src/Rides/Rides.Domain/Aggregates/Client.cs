using Rides.Domain.Events;

namespace Rides.Domain.Aggregates;

public sealed class Client : Aggregate
{
    private string _id;

    public override string Id => _id;

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }

    public static Client Create(string id, string firstName, string lastName, DateOnly birthDate)
    {
        var inst = new Client();
        
        inst.Apply(new ClientEvents.V1.ClientCreated
        {
            ClientId = id,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate
        });

        return inst;
    }
    
    protected override void When(DomainEventBase evt)
    {
        switch (evt)
        {
            case ClientEvents.V1.ClientCreated created:
                _id = created.ClientId;
                FirstName = created.FirstName;
                LastName = created.LastName;
                BirthDate = created.BirthDate;
                break;
            default:
                throw new NotSupportedException(
                    $"Event of type {evt.GetType().FullName} is not supported by {nameof(Client)} aggregate.");
        }
    }
}