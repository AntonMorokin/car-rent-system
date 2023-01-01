using MongoDB.Driver;
using Rides.Persistence.Events;
using Aggregate = Rides.Domain.Aggregates.Ride;
using View = Rides.Persistence.Views.Ride;
using RideEventsV1 = Rides.Domain.Events.RideEvents.V1;

namespace Rides.Persistence.Listeners;

internal sealed class RidesChangesListener : EventChangesListener<Aggregate, View>
{
    public RidesChangesListener(IMongoClient mongoClient)
        : base(mongoClient)
    {
    }

    protected override Task<View> UpdateViewAsync(View view, ChangeStreamDocument<EventEnvelope> change)
    {
        var evt = change.FullDocument;
        switch (evt.Payload)
        {
            case RideEventsV1.RideCreated created:
                view.RideId = created.RideId;
                view.ClientId = created.ClientId;
                view.CarId = created.CarId;
                view.CreatedTime = created.CreatedTime;
                view.Status = created.Status.ToString();
                break;
            case RideEventsV1.RideStarted started:
                view.StartedTime = started.StartedTime;
                view.Status = started.Status.ToString();
                break;
            case RideEventsV1.RideFinished finished:
                view.FinishedTime = finished.FinishedTime;
                view.OdometerReading = finished.OdometerReading;
                view.Status = finished.Status.ToString();
                break;
            case RideEventsV1.RideCancelled cancelled:
                view.FinishedTime = cancelled.CancelledTime;
                view.CancellationReason = cancelled.Reason;
                view.Status = cancelled.Status.ToString();
                break;
        }

        return Task.FromResult(view);
    }
}