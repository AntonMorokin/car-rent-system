using Rides.Domain;

namespace Rides.Persistence;

internal static class DbNamesMapper
{
    private static readonly Dictionary<Type, string> CollectionNameMap = new(4);
    private static readonly Dictionary<Type, string> AggregateNameMap = new(4);

    static DbNamesMapper()
    {
        Map<Ride>("rides", "ride");
    }

    public static void Map<T>(string collectionName, string aggregateName) where T : Aggregate
    {
        var type = typeof(T);
        CollectionNameMap.Add(type, collectionName);
        AggregateNameMap.Add(type, aggregateName);
    }

    public static string GetCollectionName<T>() where T : Aggregate
    {
        return CollectionNameMap[typeof(T)];
    }

    public static string GetAggregateName<T>() where T : Aggregate
    {
        return AggregateNameMap[typeof(T)];
    }
}