using MediatR;
using Rides.Services.Queries;

namespace Rides.Services.Handlers;

internal sealed class GetNextRideIdQueryHandler : IRequestHandler<GetNextRideIdQuery, string>
{
    private readonly IRidesService _ridesService;

    public GetNextRideIdQueryHandler(IRidesService ridesService)
    {
        _ridesService = ridesService;
    }

    public Task<string> Handle(GetNextRideIdQuery request, CancellationToken cancellationToken)
    {
        return _ridesService.GetNextIdAsync();
    }
}