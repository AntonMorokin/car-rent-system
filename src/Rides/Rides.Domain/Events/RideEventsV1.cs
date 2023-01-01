using Rides.Domain.Aggregates;

namespace Rides.Domain.Events;

public static class RideEvents
{
    public static class V1
    {
        public record RideCreated : DomainEventBase
        {
            public string RideId { get; init; }

            public string ClientId { get; init; }

            public string CarId { get; init; }

            public DateTimeOffset CreatedTime { get; init; }

            public RideStatus Status { get; set; }
        }

        public record RideStarted : DomainEventBase
        {
            public DateTimeOffset StartedTime { get; init; }

            public RideStatus Status { get; set; }
        }

        public record RideFinished : DomainEventBase
        {
            public DateTimeOffset FinishedTime { get; init; }

            public float OdometerReading { get; init; }

            public RideStatus Status { get; set; }
        }

        public record RideCancelled : DomainEventBase
        {
            public DateTimeOffset CancelledTime { get; init; }

            public string Reason { get; init; }

            public RideStatus Status { get; set; }
        }
    }
}