using Rides.Domain.Exceptions;
using Rides.Persistence;
using Rides.Persistence.Views;
using Aggregate = Rides.Domain.Aggregates.Ride;
using Model = Rides.Domain.Model.Ride;

namespace Rides.Services;

public sealed class RidesReadService : IRidesReadService
{
    private readonly IEventStore<Aggregate> _eventStore;
    private readonly IModelReader<Model> _modelReader;

    public RidesReadService(IEventStore<Aggregate> eventStore, IModelReader<Model> modelReader)
    {
        _eventStore = eventStore;
        _modelReader = modelReader;
    }

    public Task<string> GetNextIdAsync()
    {
        return _eventStore.GetNextIdAsync();
    }

    public async Task<Domain.Model.Ride> GetRideByIdAsync(string rideId)
    {
        var model = await _modelReader.LoadModelByIdOrDefault(rideId);
        return model
               ?? throw new DomainException(
                   ErrorCodes.EntityNotFound,
                   $"The ride with Id={rideId} doesn't exists");
    }
}