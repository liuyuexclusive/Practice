using LY.Common.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace LY.Application.Api
{
    [HttpHost("http://localhost:9000")]
    public interface ISysApi : IHttpApi
    {
        [HttpGet("/Schedule/GetList")]
        Task<OutputList<object>> GetList();            
    }    
}
