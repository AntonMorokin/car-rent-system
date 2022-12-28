using System;

namespace Rides.WebApi.Contract.Requests;

public record CreateRideRequest(string RideId, string ClientId, string CarId, DateTimeOffset CreatedTime);