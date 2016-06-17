using System;

namespace LY.Domain
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IUnitOfWork
    {
        void RegisterAdded(EntityBase entityBase);
        void RegisterChangeded(EntityBase entityBase);
        void RegisterRemoved(EntityBase entityBase);
        void Commit();
    }
}
