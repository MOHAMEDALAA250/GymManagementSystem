using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        #region Relations

        #region Membership

        public ICollection<MemberShip> MemberShips { get; set; }

        #endregion

        #endregion
    }
}
