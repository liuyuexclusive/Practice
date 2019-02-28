using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.Common
{
    public static class IQueryableExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sourceData">数据源</param>
        /// <param name="value">分页参数（当前页，每页大小）</param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> sourceData, IPageInput value) where T : new()
        {
           return sourceData.Skip(value.CurrentPageSize * (value.CurrentPage - 1)).Take(value.CurrentPageSize);
        }
    }
}
