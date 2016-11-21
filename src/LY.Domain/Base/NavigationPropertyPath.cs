using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LY.Domain
{
    /// <summary>
    /// 导航属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NavigationPropertyPath<TEntity>
    {
        public NavigationPropertyPath(Expression<Func<TEntity, object>> include, params Expression<Func<object, object>>[] thenIncludes)
        {
            Include = include;
            ThenIncludes = thenIncludes;
        }
        public Expression<Func<TEntity, object>> Include { get; }
        public Expression<Func<object, object>>[] ThenIncludes { get; }
    }
}
