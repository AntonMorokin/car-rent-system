namespace Rides.Services;

public interface IRidesWriteService
{
    Task CreateRideAsync(string rideId, string clientId, string carId, DateTimeOffset createdTime);
    Task StartRideAsync(string rideId, DateTimeOffset startedTime);
}