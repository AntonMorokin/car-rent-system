using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

public sealed class CancelRideCommandHandler : IRequestHandler<CancelRideCommand>
{
    private readonly IRidesService _ridesService;

    public CancelRideCommandHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public async Task<Unit> Handle(CancelRideCommand request, CancellationToken cancellationToken)
    {
        await _ridesService.CancelRideAsync(request.RideId, request.CancelledTime, request.Reason);
        return Unit.Value;
    }
}