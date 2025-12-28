using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberShip : BaseEntity
    {



        #region Member

        public Member Member { get; set; }
        public int MemberId { get; set; }

        #endregion

        #region Plan

        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        #endregion

        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            }
        }

    }
}
