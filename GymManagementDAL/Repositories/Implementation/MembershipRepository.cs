using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Implementation
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;
        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<MemberShip> GetAllMembershipsWithMemberAndPlan()
        {
            return _dbContext.MemberShips
                .Include(X => X.Member)
                .Include(X => X.Plan)
                .ToList();
        }


    }
}
