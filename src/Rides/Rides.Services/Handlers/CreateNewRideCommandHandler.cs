using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

internal sealed class CreateNewRideCommandHandler : IRequestHandler<CreateNewRideCommand>
{
    private readonly IRidesService _ridesService;

    public CreateNewRideCommandHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public async Task<Unit> Handle(CreateNewRideCommand request, CancellationToken cancellationToken)
    {
        await _ridesService.CreateRideAsync(request.RideId, request.ClientId, request.CarId, request.CreatedTime);
        return Unit.Value;
    }
}