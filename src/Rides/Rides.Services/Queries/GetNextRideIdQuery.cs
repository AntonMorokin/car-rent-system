using MediatR;

namespace Rides.Services.Queries;

public sealed class GetNextRideIdQuery : IRequest<string>
{
}