using MediatR;
using Rides.Services.Commands;

namespace Rides.Services.Handlers;

internal sealed class CreateNewRideCommandHandler : IRequestHandler<CreateNewRideCommand>
{
    private readonly IRidesWriteService _writeService;

    public CreateNewRideCommandHandler(IRidesWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(CreateNewRideCommand request, CancellationToken cancellationToken)
    {
        await _writeService.CreateRideAsync(request.RideId, request.ClientId, request.CarId, request.CreatedTime);
        return Unit.Value;
    }
}