using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;

namespace Rides.Persistence.Serialization;

public sealed class DomainEventDiscriminatorConvention : IDiscriminatorConvention
{
    private static readonly Assembly DomainAssembly = typeof(Domain.Events.RideEvents).Assembly;

    public Type GetActualType(IBsonReader bsonReader, Type nominalType)
    {
        var bookmark = bsonReader.GetBookmark();
        try
        {
            bsonReader.ReadStartDocument();
            if (!bsonReader.FindElement(ElementName))
                throw new InvalidOperationException("Unable to find discriminator property");

            var discriminatorType = bsonReader.ReadString();
            if (string.IsNullOrEmpty(discriminatorType))
            {
                throw new InvalidOperationException("Discriminator property is empty");
            }

            return DomainAssembly.GetType(discriminatorType)
                   ?? throw new InvalidOperationException(
                       $"Unable to find type {discriminatorType} in assembly {DomainAssembly.FullName}");
        }
        finally
        {
            bsonReader.ReturnToBookmark(bookmark);
        }
    }

    public BsonValue GetDiscriminator(Type nominalType, Type actualType)
    {
        var type = actualType.FullName;
        return new BsonString(type);
    }

    public string ElementName => "_t";
}