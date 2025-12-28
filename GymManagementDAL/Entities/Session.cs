using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Relations

        #region Category - Session

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        #endregion

        #region Session - trainer

        public Trainer Trainer { get; set; }

        public int TrainerId { get; set; }

        #endregion

        #region Session -  members

        public ICollection<MemberSession> MemberSessions { get; set; }

        #endregion

        #endregion

    }
}
