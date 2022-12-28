using Rides.Domain;

namespace Rides.Persistence;

internal static class DbCollectionMapper
{
    private static readonly Dictionary<Type, string> CollectionNameMap = new(4);

    static DbCollectionMapper()
    {
        Map<Ride>("rides");
    }

    public static void Map<T>(string collectionName) where T : Aggregate
    {
        CollectionNameMap.Add(typeof(T), collectionName);
    }

    public static string GetCollectionName<T>() where T : Aggregate
    {
        return CollectionNameMap[typeof(T)];
    }
}