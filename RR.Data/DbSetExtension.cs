using System.Linq.Dynamic.Core;

namespace RR.Data;

public static class DbSetExtension
{
    public static async Task<List<R>> GetPagedAsync<R, F>(this IQueryable<R> query, SortPageFilter<F> sortPageFilter) where R : class where F : class
    {
        var filter = sortPageFilter.Filter;
        var parsingConfig = new ParsingConfig
        {
            AllowNewToEvaluateAnyType = false,
            ResolveTypesBySimpleName = false,
            UseParameterizedNamesInDynamicQuery = true
        };
        if (filter != null)
        {
            var filterProps = typeof(F).GetProperties();
            foreach (var prop in filterProps)
            {
                var value = prop.GetValue(filter);
                if (value == null)
                    continue;
                switch (Type.GetTypeCode(prop.PropertyType))
                {
                    case TypeCode.String:
                        value = value.ToString()!.Replace("'", "''");
                        value = $"%{value}%";
                        query = query.Where(parsingConfig, $"{prop.Name}.Contains(@0)", value);
                        break;
                    default:
                        // For other types, use equality
                        query = query.Where(parsingConfig, $"{prop.Name} == @0", value);
                        break;
                }
            }
        }
        // Optionally add sorting
        if (sortPageFilter.Sorts != null)
        {
            foreach (var sort in sortPageFilter.Sorts)
            {
                query = query.OrderBy(parsingConfig, $"{sort.SortByField} {sort.SortDirection}");
            }
        }
        // Optionally add paging
        if (sortPageFilter.PageNumber.HasValue && sortPageFilter.PageSize.HasValue)
        {
            int skip = (sortPageFilter.PageNumber.Value - 1) * sortPageFilter.PageSize.Value;
            query = query.Skip(skip).Take(sortPageFilter.PageSize.Value);
        }
        return await query.AsNoTracking().ToListAsync();
    }
}
