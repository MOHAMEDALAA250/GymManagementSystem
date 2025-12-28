using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Implementation
{
    public class MemberSessionRepository : GenericRepository<MemberSession>, IMemberSessionRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberSessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MemberSession> GetBySessionWithAllMembers(int sessionId)
        {
            return _dbContext.MemberSessions
                .Include(M => M.Member)
                .Where(S => S.SessionId == sessionId)
                .ToList();
        }

        public MemberSession? GetMemberSessionBySessionAndMember(int sessionId, int memberId)
        {
            return _dbContext.MemberSessions
                .FirstOrDefault(m => m.SessionId == sessionId && m.MemberId == memberId);

            ;
        }
    }
}
