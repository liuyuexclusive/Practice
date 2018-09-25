using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Common
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> sourceData, IPageInput value) where T : new()
        {
           return sourceData.Skip(value.CurrentPageSize * (value.CurrentPage - 1)).Take(value.CurrentPageSize);
        }
    }
}
