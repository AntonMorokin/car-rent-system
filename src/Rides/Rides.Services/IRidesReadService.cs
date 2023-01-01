using Rides.Domain.Model;

namespace Rides.Services;

internal interface IRidesReadService
{
    Task<string> GetNextIdAsync();

    Task<Ride> GetRideByIdAsync(string rideId);
}