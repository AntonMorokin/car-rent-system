using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

internal sealed class StartRideCommandHandler : IRequestHandler<StartRideCommand>
{
    private readonly IRidesWriteService _writeService;

    public StartRideCommandHandler(IRidesWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(StartRideCommand request, CancellationToken cancellationToken)
    {
        await _writeService.StartRideAsync(request.RideId, request.StartedTime);
        return Unit.Value;
    }
}