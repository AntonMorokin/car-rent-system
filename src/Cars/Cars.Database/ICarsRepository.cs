using System.Threading;
using System.Threading.Tasks;
using Cars.Model;

namespace Cars.Database;

public interface ICarsRepository
{
    Task<string> CreateNewCarAsync(string number,
        string brand,
        string model,
        float mileage,
        CarStatus status,
        CancellationToken cancellationToken);

    Task<Car> GetCarByIdAsync(string id, CancellationToken cancellationToken);

    Task<Car> GetCarByNumberAsync(string number, CancellationToken cancellationToken);

    Task UpdateCarAsync(string carId,
        string? number,
        string? brand,
        string? model,
        float? mileage,
        CarStatus? status,
        CancellationToken cancellationToken);
}