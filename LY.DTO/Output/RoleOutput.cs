using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Output
{
    public class RoleOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { set; get; }

    }
}
