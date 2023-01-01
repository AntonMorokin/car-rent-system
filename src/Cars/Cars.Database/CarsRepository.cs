using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cars.Model;
using Dapper;
using Npgsql;

namespace Cars.Database;

internal sealed class CarsRepository : ICarsRepository
{
    private readonly string _connectionString;

    public CarsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string> CreateNewCarAsync(string number, string brand, string model, float mileage,
        CancellationToken cancellationToken)
    {
        const string Command = "insert into cars_ms.cars(number, brand, model, mileage)"
                               + " values(@number, @brand, @model, @mileage) returning id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var @params = new
        {
            number,
            brand,
            model,
            mileage
        };

        return await connection.QuerySingleAsync<string>(Command, @params);
    }

    public async Task<Car> GetCarByIdAsync(string id, CancellationToken cancellationToken)
    {
        const string Command =
            "select id as \"Id\", number as \"Number\", brand as \"Brand\", model as \"Model\", mileage as \"Mileage\""
            + " from cars_ms.cars where id = @id";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var @params = new
        {
            id = Convert.ToInt32(id)
        };

        return await GetSingleCarWithCheckingAsync(connection, Command, @params, $"id={id}", nameof(id));
    }

    private static async Task<Car> GetSingleCarWithCheckingAsync(IDbConnection connection,
        string command,
        object parameters,
        string parameterDescription,
        string parameterName)
    {
        var result = await connection.QueryAsync<Model.Car>(command, parameters);
        using var enumerator = result.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new ArgumentException($"There is no Car with {parameterDescription}", parameterName);
        }

        var car = enumerator.Current;

        if (enumerator.MoveNext())
        {
            throw new ArgumentException($"There are several Cars with {parameterDescription}", parameterName);
        }

        return new Car
        {
            Id = car.Id.ToString(),
            Number = car.Number,
            Brand = car.Brand,
            Model = car.Model,
            Mileage = car.Mileage
        };
    }

    public async Task<Car> GetCarByNumberAsync(string number, CancellationToken cancellationToken)
    {
        const string Command =
            "select id as \"Id\", number as \"Number\", brand as \"Brand\", model as \"Model\", mileage as \"Mileage\""
            + " from cars_ms.cars where number = @number";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var @params = new
        {
            number
        };

        return await GetSingleCarWithCheckingAsync(connection, Command, @params, $"number={number}", nameof(number));
    }
}