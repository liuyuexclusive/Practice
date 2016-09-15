using LY.Domain.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Domain;
using Microsoft.EntityFrameworkCore;

namespace LY.EFRepository.Sys
{
    public class RoleRepo : Repository<Role>, IRoleRepo
    {
        public RoleRepo(IUnitOfWork unitOfWork, DbContext dbContext) : base(unitOfWork, dbContext)
        {
        }

        public IList<Role> QueryInclude()
        {
            return base.Entities.Include(x => x.RoleUserMappingList).ThenInclude(x => x.User).ToList();
        }
    }
}
