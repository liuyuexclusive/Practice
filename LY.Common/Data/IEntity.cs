using System;
namespace LY.Common
{
    public interface IEntity
    {
        int ID { get; set; }
        DateTime CreatedOn { get; set; } 
        string CreatedBy { get; set; }
        long Series { get; set; }
    }
}
