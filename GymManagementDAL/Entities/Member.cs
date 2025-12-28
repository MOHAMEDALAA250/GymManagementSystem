using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        #region Relations


        #region Member Has HealthRecod 1-1

        public HealthRecord HealthRecord { get; set; }

        #endregion

        #region Member has memberships

        public ICollection<MemberShip> MemberShips { get; set; }
        #endregion


        #region Member - Sessions

        public ICollection<MemberSession> Sessions { get; set; }

        #endregion

        #endregion
    }
}
