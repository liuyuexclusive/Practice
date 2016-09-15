using System;

namespace LY.Domain
{
    /// <summary>
    /// 代表一个工作单元。
    /// </summary>
    public interface IBaseUnitOfWork<Tkey>
    {
        void RegisterAdded(BaseEntity<Tkey> entityBase);
        void RegisterChangeded(BaseEntity<Tkey> entityBase);
        void RegisterRemoved(BaseEntity<Tkey> entityBase);
        void Commit();
    }
}
