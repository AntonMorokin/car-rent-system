using MediatR;
using Rides.Domain.Views;

namespace Rides.Services.Queries;

public sealed class GetRideByIdQuery : IRequest<Ride>
{
    public string RideId { get; set; }
}