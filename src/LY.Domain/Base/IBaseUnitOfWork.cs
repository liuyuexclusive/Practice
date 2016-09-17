using System;

namespace LY.Domain
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IBaseUnitOfWork<Tkey>
    {
        void RegisterAdded(BaseEntity<Tkey> entityBase);
        void RegisterUpdated(BaseEntity<Tkey> entityBase);
        void RegisterDeleted(BaseEntity<Tkey> entityBase);
        void Commit();
    }
}
