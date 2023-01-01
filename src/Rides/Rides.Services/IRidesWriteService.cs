namespace Rides.Services;

internal interface IRidesWriteService
{
    Task CreateRideAsync(string rideId, string clientId, string carId, DateTimeOffset createdTime);
    Task StartRideAsync(string rideId, DateTimeOffset startedTime);
}