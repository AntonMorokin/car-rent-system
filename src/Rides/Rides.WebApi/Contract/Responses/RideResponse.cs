using System;
using System.Runtime.InteropServices.JavaScript;

namespace Rides.WebApi.Contract.Responses;

public record RideResponse(string Id,
    string ClientId,
    string CarId,
    string Status,
    DateTimeOffset CreatedTime,
    DateTimeOffset? StartedTime,
    DateTimeOffset? FinishedTime,
    float? OdometerReading,
    string? CancellationReason);