using System.Threading;
using System.Threading.Tasks;
using Clients.Services;
using Clients.WebApi.Contract.Requests;
using Clients.WebApi.Contract.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Clients.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpPut]
    public Task<string> CreateNewClient([FromBody] CreateNewClientRequest request, CancellationToken cancellationToken)
    {
        return _clientsService.CreateNewClientAsync(request.FirstName, request.LastName, request.BirthDate, cancellationToken);
    }

    [HttpGet("{clientId}")]
    public async Task<ClientResponse> GetClientById([FromRoute] string clientId, CancellationToken cancellationToken)
    {
        var client = await _clientsService.GetClientByAsync(clientId, cancellationToken);
        return new ClientResponse(client.Id, client.FirstName, client.LastName, client.BirthDate);
    }
}