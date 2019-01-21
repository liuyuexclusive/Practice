namespace LY.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class Entity
    {

        public int ID { get; set; }

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
