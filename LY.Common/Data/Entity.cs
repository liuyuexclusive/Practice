using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.Common
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class Entity : IEntity
    {

        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CreatedBy { get; set; }

        public long Series { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
            {
                return false;
            }
            return (this == (Entity)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }
    }
}
