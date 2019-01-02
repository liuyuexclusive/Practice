namespace LY.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class Entity
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public override bool Equals(object entity)
        {
            if (entity == null || !(entity is Entity))
            {
                return false;
            }
            return (this == (Entity)entity);
        }
    }
}
