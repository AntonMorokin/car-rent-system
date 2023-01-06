using Cars.Database;
using Cars.Model;
using Core.Messaging;
using Core.Messaging.Events;

namespace Cars.Services;

internal sealed class CarsService : ICarsService
{
    private readonly ICarsRepository _carsRepository;
    private readonly IMessageProducer _messageProducer;

    public CarsService(ICarsRepository carsRepository, IMessageProducer messageProducer)
    {
        _carsRepository = carsRepository;
        _messageProducer = messageProducer;
    }

    public async Task<string> CreateNewCarAsync(string number,
        string brand,
        string model,
        float mileage,
        CancellationToken cancellationToken)
    {
        var carId = await _carsRepository.CreateNewCarAsync(number,
            brand,
            model,
            mileage,
            CarStatus.Ready,
            cancellationToken);
        
        await _messageProducer.SendAsync(Consts.Topics.Cars, new CarEvents.V1.CarCreated
        {
            CarId = carId,
            Brand = brand,
            Model = model,
            Number = number
        });

        return carId;
    }

    public Task<Car> GetCarByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _carsRepository.GetCarByIdAsync(id, cancellationToken);
    }

    public Task<Car> GetCarByNumberAsync(string number, CancellationToken cancellationToken)
    {
        return _carsRepository.GetCarByNumberAsync(number, cancellationToken);
    }

    public async Task UseInRideAsync(string carId, string rideId, CancellationToken cancellationToken)
    {
        var car = await GetCarByIdAsync(carId, cancellationToken);
        if (car.Status != CarStatus.Ready)
        {
            throw new InvalidOperationException(
                $"The car with id={car.Id} isn't ready to be used in the ride with id={rideId}");
        }

        await _carsRepository.UpdateCarAsync(carId,
            null,
            null,
            null,
            null,
            status: CarStatus.InRide,
            cancellationToken: cancellationToken);

        await _messageProducer.SendAsync(Consts.Topics.Cars, new CarEvents.V1.CarHeld
        {
            CarId = car.Id,
            RideId = rideId
        });
    }
}