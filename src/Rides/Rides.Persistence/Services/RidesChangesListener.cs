using MongoDB.Driver;
using Rides.Persistence.Events;
using Aggregate = Rides.Domain.Aggregates.Ride;
using View = Rides.Persistence.Views.Ride;
using RideEventsV1 = Rides.Domain.Events.RideEvents.V1;

namespace Rides.Persistence.Services;

internal sealed class RidesChangesListener : EventChangesListener<Aggregate, View>
{
    public RidesChangesListener(IMongoClient mongoClient)
        : base(mongoClient)
    {
    }

    protected override Task<View> UpdateModelAsync(View model, ChangeStreamDocument<EventEnvelope> change)
    {
        var evt = change.FullDocument;
        switch (evt.Payload)
        {
            case RideEventsV1.RideCreated created:
                model.RideId = created.RideId;
                model.ClientId = created.ClientId;
                model.CarId = created.CarId;
                model.CreatedTime = created.CreatedTime;
                model.Status = Domain.Aggregates.RideStatus.Created.ToString();
                break;
            case RideEventsV1.RideStarted started:
                model.StartedTime = started.StartedTime;
                model.Status = Domain.Aggregates.RideStatus.InProgress.ToString();
                break;
        }

        return Task.FromResult(model);
    }
}