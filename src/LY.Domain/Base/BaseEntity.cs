namespace LY.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class BaseEntity<Tkey>
    {
        private Tkey _id;
        
        public Tkey ID
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public override bool Equals(object entity)
        {
            if (entity == null || !(entity is BaseEntity<Tkey>))
            {
                return false;
            }
            return (this == (BaseEntity<Tkey>)entity);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
