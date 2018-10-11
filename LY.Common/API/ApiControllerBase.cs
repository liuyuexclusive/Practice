using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LY.Common.API
{
    public class ApiControllerBase: ControllerBase
    {
        /// <summary>
        /// OK
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        protected async Task<Output> OK(string message = "")
        {
            return await Task.Run(() => {
                return new Output() { Message = message };
            });
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message">message</param>
        /// <returns></returns>
        protected async Task<Output<T>> OK<T>(T t, string message = "")
        {
            return await Task.Run(() => {
                return new Output<T>() { Data = t , Message = message };
            });
        }


        /// <summary>
        /// OKList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">list</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        protected async Task<OutputList<T>> OKList<T>(IEnumerable<T> list, int? total = null)
        {
            return await Task.Run(() => {
                return new OutputList<T>() { Data = list, Total = total };
            });
        }
    }
}
