using System;
using System.Collections.Generic;
using System.Text;

namespace LY.Common
{
    public interface IPageInput
    {
        int CurrentPage { get; set; }
        int CurrentPageSize { get; set; }
    }
}
