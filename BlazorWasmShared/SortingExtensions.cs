using System;
using System.Collections.Generic;
using System.Linq;
using BlazorWasmShared.Models;

namespace BlazorWasmShared
{
    public static class SortingExtensions
    {
        public static IEnumerable<T> OrderByMultiple<T>(this IEnumerable<T> source, List<SortableColumn> columns)
        {
            IOrderedEnumerable<T>? orderedQuery = null;
            foreach (var column in columns)
            {
                if (orderedQuery == null)
                {
                    orderedQuery = column.SortOrder == SortDirection.Ascending
                        ? source.OrderBy(x => GetPropertyValue(x, column.PropertyName))
                        : source.OrderByDescending(x => GetPropertyValue(x, column.PropertyName));
                }
                else
                {
                    orderedQuery = column.SortOrder == SortDirection.Ascending
                        ? orderedQuery.ThenBy(x => GetPropertyValue(x, column.PropertyName))
                        : orderedQuery.ThenByDescending(x => GetPropertyValue(x, column.PropertyName));
                }
            }
            return orderedQuery ?? source;
        }

        private static object GetPropertyValue<T>(T obj, string propertyName)
        {
            return obj?.GetType().GetProperty(propertyName)?.GetValue(obj, null) ?? string.Empty;
        }
    }
}
