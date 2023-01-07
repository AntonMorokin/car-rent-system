using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

internal sealed class FinishRideCommandHandler : IRequestHandler<FinishRideCommand>
{
    private readonly IRidesService _ridesService;

    public FinishRideCommandHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public async Task<Unit> Handle(FinishRideCommand request, CancellationToken cancellationToken)
    {
        await _ridesService.FinishRideAsync(request.RideId, request.FinishedTime, request.OdometerReading);
        return Unit.Value;
    }
}