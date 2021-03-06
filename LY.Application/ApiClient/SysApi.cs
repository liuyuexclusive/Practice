﻿using LY.Common.API;
using LY.DTO.Output;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace LY.Application.ApiClient
{
    [HttpHost("http://192.168.123.6:9000")]
    public interface ISysApi : IHttpApi
    {
        [HttpGet("/Role/GetList")]
        Task<OutputList<RoleOutput>> GetRoleList([Header("Authorization")] string authorization);
        //HttpApiClient.Create<ISysApi>().GetRoleList(HttpContext.Request.Headers["Authorization"])
    }    
}
