using LY.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.DTO.Input
{
    public class BaseQueryInput: IPageInput
    {
        public int CurrentPage { get; set; }
        public int CurrentPageSize { get; set; }
    }
}
