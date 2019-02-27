using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common
{
    public interface IEntityCache<TEntity> where TEntity : Entity, IEntityCacheable
    {
        IList<TEntity> List();
    }
}
