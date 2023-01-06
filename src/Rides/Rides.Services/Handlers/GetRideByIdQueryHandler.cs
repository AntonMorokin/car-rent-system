using MediatR;
using Rides.Domain.Views;
using Rides.Services.Queries;

namespace Rides.Services.Handlers;

internal sealed class GetRideByIdQueryHandler : IRequestHandler<GetRideByIdQuery, Ride>
{
    private readonly IRidesService _ridesService;

    public GetRideByIdQueryHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public Task<Ride> Handle(GetRideByIdQuery request, CancellationToken cancellationToken)
    {
        return _ridesService.GetRideByIdAsync(request.RideId);
    }
}