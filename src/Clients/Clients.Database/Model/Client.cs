﻿namespace Clients.Database.Model;

internal class Client
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }
}