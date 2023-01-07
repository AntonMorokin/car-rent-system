using Rides.Domain.Views;

namespace Rides.Services;

public interface IRidesService
{
    Task<string> GetNextIdAsync();
    Task<Ride> GetRideByIdAsync(string rideId);
    Task CreateRideAsync(string rideId, string clientId, string carId, DateTimeOffset createdTime);
    Task StartRideAsync(string rideId, DateTimeOffset startedTime);
    Task FinishRideAsync(string rideId, DateTimeOffset finishedTime, float odometerReading);
    Task CancelRideAsync(string rideId, DateTimeOffset cancelledTime, string reason);
}