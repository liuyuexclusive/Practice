using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.WebAPI
{
    public class ApiControllerBase: ControllerBase
    {
        /// <summary>
        /// OK
        /// </summary>
        /// <returns></returns>
        public async Task<Output> OK()
        {
            return await Task.Run(() => {
                return new Output() { Message="操作成功" };
            });
        }
    }
}
