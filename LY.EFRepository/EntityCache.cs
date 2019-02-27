using LY.Common;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LY.EFRepository
{
    public class EntityCache<TEntity> : IEntityCache<TEntity> where TEntity : Entity, IEntityCacheable
    {
        public IDistributedCache Cache { get; set; }

        public EntityCache()
        {

        }

        public IList<TEntity> List()
        {
            var typeName = typeof(TEntity).FullName;
            var strResult = Cache.GetString(typeName);
            if (string.IsNullOrEmpty(strResult))
            {
                var result = IOCManager.Resolve<IRepository<TEntity>, IList<TEntity>>(x => x.Queryable.ToArray());
                var str = JsonConvert.SerializeObject(result);
                Cache.SetString(typeName, str);
                return result;
            }
            else
            {
                return JsonConvert.DeserializeObject<IList<TEntity>>(strResult);
            }
        }
    }
}
