using System;

namespace LY.Domain
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IUnitOfWork
    {
        void RegisterAdded(Entity entity);
        void RegisterUpdated(Entity entity);
        void RegisterDeleted(Entity entity);
        void Commit();
    }
}
