using System;

namespace Clients.WebApi.Contract.Responses;

public sealed record ClientResponse(string id, string FirstName, string LastName, DateOnly BirthDate);