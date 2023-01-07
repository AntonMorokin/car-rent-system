namespace Rides.Messaging.Services;

internal interface IClientsService
{
    Task CreateClientAsync(string clientId, string firstName, string lastName, DateOnly birthDate);
}