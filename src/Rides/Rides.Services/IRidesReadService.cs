using Rides.Domain.Model;

namespace Rides.Services;

public interface IRidesReadService
{
    Task<string> GetNextIdAsync();

    Task<Ride> GetRideByIdAsync(string rideId);
}