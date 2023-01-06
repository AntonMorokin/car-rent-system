using Cars.Database;
using Cars.Model;
using Core.Messaging;

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
        var carId = await _carsRepository.CreateNewCarAsync(number, brand, model, mileage, cancellationToken);
        
        await _messageProducer.SendAsync(Consts.Topics.Cars, new Core.Messaging.Events.CarEvents.V1.CarCreated
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
}