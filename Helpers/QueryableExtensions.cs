using System.Linq.Expressions;

namespace ProjectAspEShop2024.Helpers
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> AppendOrderBy<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> sortSelector)
        {
            return (source as IOrderedQueryable<T>)?.ThenBy(sortSelector)
                ?? source.OrderBy(sortSelector);
        }

        public static IOrderedQueryable<T> AppendOrderByDescending<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> sortSelector)
        {
            return (source as IOrderedQueryable<T>)?.ThenByDescending(sortSelector)
                ?? source.OrderByDescending(sortSelector);
        }
    }
}
