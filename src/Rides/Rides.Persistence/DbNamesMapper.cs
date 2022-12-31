using Rides.Domain.Aggregates;

namespace Rides.Persistence;

internal static class DbNamesMapper
{
    private static readonly Dictionary<Type, string> WriteCollectionNameMap = new(4);
    private static readonly Dictionary<Type, string> ReadCollectionNameMap = new(4);
    private static readonly Dictionary<Type, string> AggregateNameMap = new(4);

    static DbNamesMapper()
    {
        Map<Ride>("rides", "rides", "ride");
    }

    public static void Map<T>(string writeCollectionName, string readCollectionName, string aggregateName) where T : Aggregate
    {
        var type = typeof(T);
        WriteCollectionNameMap.Add(type, writeCollectionName);
        ReadCollectionNameMap.Add(type, readCollectionName);
        AggregateNameMap.Add(type, aggregateName);
    }

    public static string GetWriteCollectionName<T>() where T : Aggregate
    {
        return WriteCollectionNameMap[typeof(T)];
    }
    
    public static string GetReadCollectionName<T>() where T : Aggregate
    {
        return ReadCollectionNameMap[typeof(T)];
    }

    public static string GetAggregateName<T>() where T : Aggregate
    {
        return AggregateNameMap[typeof(T)];
    }
}