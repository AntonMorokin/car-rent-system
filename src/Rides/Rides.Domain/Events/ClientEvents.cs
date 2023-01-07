namespace Rides.Domain.Events;

public static class ClientEvents
{
    public static class V1
    {
        public record ClientCreated : DomainEventBase
        {
            public string ClientId { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateOnly BirthDate { get; set; }
        }
    }
}