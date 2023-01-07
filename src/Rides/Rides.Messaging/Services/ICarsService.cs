namespace Rides.Messaging.Services;

internal interface ICarsService
{
    Task CreateCarAsync(string carId);
    Task HoldCarAsync(string carId, string rideId);
    Task FreeCarAsync(string carId);
}