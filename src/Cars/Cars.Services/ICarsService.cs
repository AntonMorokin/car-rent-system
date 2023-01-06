using Cars.Model;

namespace Cars.Services;

public interface ICarsService
{
    Task<string> CreateNewCarAsync(string number,
        string brand,
        string model,
        float mileage,
        CancellationToken cancellationToken);

    Task<Car> GetCarByIdAsync(string id, CancellationToken cancellationToken);

    Task<Car> GetCarByNumberAsync(string number, CancellationToken cancellationToken);
    Task UseInRideAsync(string carId, string rideId, CancellationToken cancellationToken);
}