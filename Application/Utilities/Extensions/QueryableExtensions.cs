using Application.DTOs.Request;
using System.Linq.Dynamic.Core;

namespace Application.Utilities.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyDynamicSorting<T>(
            this IQueryable<T> query,
            List<SortOption>? sortOptions,
            string defaultSortColumn,
            bool defaultSortDescending = false)
        {
            if (sortOptions != null && sortOptions.Any())
            {
                foreach (var sort in sortOptions)
                {
                    query = query.OrderBy($"{sort.Column} {(sort.Descending ? "descending" : "ascending")}");
                }
            }
            else
            {
                query = query.OrderBy($"{defaultSortColumn} {(defaultSortDescending ? "descending" : "ascending")}");
            }

            return query;
        }

        public static IQueryable<T> ApplyDynamicFilters<T>(
            this IQueryable<T> query,
            BaseFilterDto filterDto)
        {
            if (!string.IsNullOrWhiteSpace(filterDto.SearchTerm))
            {
                var stringProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                var predicate = string.Join(" OR ",
                    stringProperties.Select(p => $"{p.Name}.Contains(@0)"));

                if (!string.IsNullOrEmpty(predicate))
                {
                    query = query.Where(predicate, filterDto.SearchTerm);
                }
            }

            return query;
        }
    }
}
