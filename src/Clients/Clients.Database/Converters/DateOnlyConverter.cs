using System.Data;
using Dapper;

namespace Clients.Database.Converters;

public sealed class DateOnlyConverter : SqlMapper.TypeHandler<DateOnly>
{
    public static DateOnlyConverter Single = new();
    
    private readonly TimeOnly _zero = TimeOnly.MinValue;
    
    private DateOnlyConverter()
    { }
    
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(_zero);
    }

    public override DateOnly Parse(object value)
    {
        if (value is DateOnly dateOnly)
        {
            return dateOnly;
        }
        
        return DateOnly.FromDateTime(Convert.ToDateTime(value));
    }
}