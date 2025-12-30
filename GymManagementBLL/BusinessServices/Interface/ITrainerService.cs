using GymManagementBLL.View_Models.TrainerVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Interface
{
    public interface ITrainerService
    {
        IEnumerable<TrainerModelView> GetAllTrainers();

        bool CreateTrainer(CreateTrainerModelView createTrainer);

        TrainerModelView? GetTrainerDetails(int trainerId);

        TrainerToUpdateModelView? GetTrainerToUpdate(int trainerId);

        bool UpdateTrainer(int trainerId, TrainerToUpdateModelView trainerToUpdate);

        bool DeleteTrainer(int trainerId);

    }
}
