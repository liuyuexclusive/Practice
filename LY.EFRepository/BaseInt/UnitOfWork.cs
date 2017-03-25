using LY.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace LY.EFRepository
{
    [DebuggerStepThrough]
    public class UnitOfWork : BaseUnitOfWork<int>, IUnitOfWork
    {
        public UnitOfWork(LYDbContext dbContext) : base(dbContext)
        {
        }
    }
}
