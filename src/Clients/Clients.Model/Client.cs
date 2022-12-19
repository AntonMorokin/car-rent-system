namespace Clients.Model;

public class Client
{
    public string Id { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }
}