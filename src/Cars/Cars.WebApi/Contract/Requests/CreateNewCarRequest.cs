namespace Cars.WebApi.Model.Requests;

public record CreateNewCarRequest(string Number, string Brand, string Model, float Mileage);