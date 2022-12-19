using System;

namespace Clients.WebApi.Contract.Requests;

public record CreateNewClientRequest(string FirstName, string LastName, DateOnly BirthDate);