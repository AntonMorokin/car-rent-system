namespace Cars.Database.Model;

internal sealed class Car
{
    public int Id { get; set; }

    public string Number { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public float Mileage { get; set; }

    public string Status { get; set; }
}