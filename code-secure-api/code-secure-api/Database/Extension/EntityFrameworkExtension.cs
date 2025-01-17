using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CodeSecure.Database.Extension;

public static class EntityFrameworkExtension
{
    private static readonly Type FuncTtResult = typeof(Func<,>);

    public static Page<T> Page<T>(this IQueryable<T> query, int page, int size)
    {
        var skip = (page - 1) * size;
        var count = query.Count();
        return new Page<T>
        {
            Count = count,
            PageCount = (int)Math.Ceiling((double)count / size),
            Items = query.Skip(skip).Take(size).ToList(),
            CurrentPage = page,
            Size = size
        };
    }

    public static async Task<Page<T>> PageAsync<T>(this IQueryable<T> query, int page, int size)
    {
        var skip = (page - 1) * size;
        var count = query.Count();
        return new Page<T>
        {
            Count = count,
            PageCount = (int)Math.Ceiling((double)count / size),
            Items = await query.Skip(skip).Take(size).ToListAsync(),
            CurrentPage = page,
            Size = size
        };
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool desc)
    {
        var itemType = typeof(T);
        var propertyInfo = itemType.GetProperty(propertyName);
        if (propertyInfo == null) throw new NullReferenceException();
        var propertyType = propertyInfo.PropertyType;
        // Func<T,TPropertyType>
        var delegateType = FuncTtResult.MakeGenericType(itemType, propertyType);
        // T x =>
        var parameterExpression = Expression.Parameter(itemType, "x");
        // T x => x.Property
        var propertyAccess = Expression.Property(parameterExpression, propertyInfo);
        // Func<T,TPropertyType> = T x => x.Property
        var keySelector = Expression.Lambda(delegateType, propertyAccess, parameterExpression);
        // query.OrderBy(x => x.Property)
        var sortMethod = "OrderBy";
        if (desc) sortMethod = "OrderByDescending";

        var orderByExpression = Expression.Call(
            typeof(Queryable),
            sortMethod,
            new[] { source.ElementType, propertyInfo.PropertyType },
            source.Expression, keySelector);
        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(orderByExpression);
    }
}