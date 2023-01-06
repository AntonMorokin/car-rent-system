using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

internal sealed class StartRideCommandHandler : IRequestHandler<StartRideCommand>
{
    private readonly IRidesService _ridesService;

    public StartRideCommandHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public async Task<Unit> Handle(StartRideCommand request, CancellationToken cancellationToken)
    {
        await _ridesService.StartRideAsync(request.RideId, request.StartedTime);
        return Unit.Value;
    }
}