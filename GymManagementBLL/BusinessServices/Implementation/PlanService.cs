using GymManagementBLL.BusinessServices.Interface;
using GymManagementBLL.View_Models.PlanVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return plans.Select(P => new PlanViewModel
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                DurationDays = P.DurationDays,
                Price = P.Price,
                IsActive = P.IsActive
            });
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null) return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public PlanToUpdateViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false || HasActiveMemberships(planId))
                return null;

            return new PlanToUpdateViewModel
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };


        }

        public bool ToggleStatus(int planId)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(planId);
            if (plan is null || HasActiveMemberships(planId)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;

            plan.UpdatedAt = DateTime.Now;

            planRepo.Update(plan);
            return _unitOfWork.SaveChanges() > 0;

        }

        public bool UpdatePlan(int planId, PlanToUpdateViewModel planToUpdate)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(planId);
            if (plan is null || planToUpdate is null) return false;

            #region manual tuple
            (plan.Price, plan.Description, plan.DurationDays) =
                  (planToUpdate.Price, planToUpdate.Description, planToUpdate.DurationDays);
            #endregion

            try
            {
                planRepo.Update(plan);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #region Helper Methods

        private bool HasActiveMemberships(int planId)
        {
            var activeMemberShips = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(M => M.PlanId == planId && M.Status == "Active");

            return activeMemberShips.Any();
        }

        #endregion
    }
}
