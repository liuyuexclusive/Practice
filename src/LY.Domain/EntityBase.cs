namespace LY.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class EntityBase 
    {
        private int _id;
        
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public override bool Equals(object entity)
        {
            if (entity == null || !(entity is EntityBase))
            {
                return false;
            }
            return (this == (EntityBase)entity);
        }

        public static bool operator ==(EntityBase entity1, EntityBase entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            return entity1.ID == entity2.ID;
        }

        public static bool operator !=(EntityBase entity1, EntityBase entity2)
        {
            return (!(entity1 == entity2));
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
