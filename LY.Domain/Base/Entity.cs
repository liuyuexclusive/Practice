using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LY.Domain
{
    /// <summary>
    /// 实体基类。
    /// </summary>
    public abstract class Entity
    {

        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedOn { get; set; }

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
