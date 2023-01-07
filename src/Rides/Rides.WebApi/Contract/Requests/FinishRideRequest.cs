using System;

namespace Rides.WebApi.Contract.Requests;

public record FinishRideRequest(string RideId, DateTimeOffset FinishedTime, float OdometerReading);