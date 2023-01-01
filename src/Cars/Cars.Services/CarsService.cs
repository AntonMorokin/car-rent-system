using Cars.Database;
using Cars.Model;

namespace Cars.Services;

internal sealed class CarsService : ICarsService
{
    private readonly ICarsRepository _carsRepository;

    public CarsService(ICarsRepository carsRepository)
    {
        _carsRepository = carsRepository;
    }

    public Task<string> CreateNewCarAsync(string number, string brand, string model, float mileage, CancellationToken cancellationToken)
    {
        return _carsRepository.CreateNewCarAsync(number, brand, model, mileage, cancellationToken);
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