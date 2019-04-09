using System;

namespace LY.Common
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IUnitOfWork
    {
        void RegisterAdded<T>(T entity) where T : Entity;
        void RegisterUpdated(IEntity entity);
        void RegisterDeleted(IEntity entity);
        void Commit();
    }
}
