using LY.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using LY.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

namespace LY.EFRepository
{
    /// <summary>
    /// 资源库基类。
    /// </summary>
    /// <typeparam name="TEntity">实体类型。</typeparam>
    public class Repository<TEntity> : BaseRepository<int, TEntity>, IRepository<TEntity> where TEntity : Entity
    {
        public Repository(IUnitOfWork unitOfWork, DbContext dbContext) : base(unitOfWork, dbContext)
        {
        }
    }
}
