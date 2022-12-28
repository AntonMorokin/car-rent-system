using System;

namespace Rides.WebApi.Contract.Requests;

public record StartRideRequest(string RideId, DateTimeOffset StartedTime);