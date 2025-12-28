using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberSession : BaseEntity
    {
     


        #region Relations

        #region Member

        public int MemberId { get; set; }
        public Member Member { get; set; }

        #endregion

        #region Session

        public int SessionId { get; set; }
        public Session Session { get; set; }

        #endregion

        #endregion

        public bool IsAttended { get; set; }
    }
}
