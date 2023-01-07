using System;

namespace Rides.WebApi.Contract.Requests;

public record CancelRideRequest(string RideId, DateTimeOffset CancelledTime, string Reason);