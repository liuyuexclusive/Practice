using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LY.EFRepository
{
    public interface IEntityToTable
    {
        void Excute(ModelBuilder modelBuilder);
    }
}
