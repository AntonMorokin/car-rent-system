using MediatR;
using Rides.Services.Queries;

namespace Rides.Services.Handlers;

public sealed class GetNextRideIdQueryHandler : IRequestHandler<GetNextRideIdQuery, string>
{
    private readonly IRidesReadService _readService;

    public GetNextRideIdQueryHandler(IRidesReadService readService)
    {
        _readService = readService;
    }

    public Task<string> Handle(GetNextRideIdQuery request, CancellationToken cancellationToken)
    {
        return _readService.GetNextIdAsync();
    }
}