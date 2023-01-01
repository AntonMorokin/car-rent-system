using MediatR;
using Rides.Domain.Model;
using Rides.Services.Queries;

namespace Rides.Services.Handlers;

public sealed class GetRideByIdQueryHandler : IRequestHandler<GetRideByIdQuery, Ride>
{
    private readonly IRidesReadService _readService;

    public GetRideByIdQueryHandler(IRidesReadService readService)
    {
        _readService = readService;
    }

    public Task<Ride> Handle(GetRideByIdQuery request, CancellationToken cancellationToken)
    {
        return _readService.GetRideByIdAsync(request.RideId);
    }
}