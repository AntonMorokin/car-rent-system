using Rides.Domain.Aggregates;
using Rides.Domain.Views;

namespace Rides.Persistence;

internal static class DbNamesMapper
{
    private static readonly Dictionary<Type, string> WriteCollectionNameMap = new(4);
    private static readonly Dictionary<Type, string> ReadCollectionNameMap = new(4);
    private static readonly Dictionary<Type, string> AggregateNameMap = new(4);

    static DbNamesMapper()
    {
        Map<Domain.Aggregates.Ride, Domain.Views.Ride>("rides", "rides", "ride");
        Map<Car>("cars", "car");
        Map<Client>("clients", "client");
    }
    
    private static void Map<TAgg>(string writeCollectionName, string aggregateName)
        where TAgg : Aggregate
    {
        var aggregateType = typeof(TAgg);

        WriteCollectionNameMap.Add(aggregateType, writeCollectionName);
        AggregateNameMap.Add(aggregateType, aggregateName);
    }

    private static void Map<TAgg, TView>(string writeCollectionName, string readCollectionName, string aggregateName)
        where TAgg : Aggregate
        where TView : ViewBase
    {
        var aggregateType = typeof(TAgg);
        var viewType = typeof(TView);

        WriteCollectionNameMap.Add(aggregateType, writeCollectionName);
        ReadCollectionNameMap.Add(viewType, readCollectionName);
        AggregateNameMap.Add(aggregateType, aggregateName);
    }

    public static string GetWriteCollectionName<T>() where T : Aggregate
    {
        return WriteCollectionNameMap[typeof(T)];
    }
    
    public static string GetReadCollectionName<T>() where T : ViewBase
    {
        return ReadCollectionNameMap[typeof(T)];
    }

    public static string GetAggregateName<T>() where T : Aggregate
    {
        return AggregateNameMap[typeof(T)];
    }
}