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
    Task UseCarInRideAsync(string carId, string rideId, CancellationToken cancellationToken);
    Task FinishRideAsync(string carId, float odometerReading, CancellationToken cancellationToken);
    Task CancelRideAsync(string carId, CancellationToken cancellationToken);
}