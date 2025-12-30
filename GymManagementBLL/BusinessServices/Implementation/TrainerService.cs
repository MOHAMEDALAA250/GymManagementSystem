using GymManagementBLL.BusinessServices.Interface;
using GymManagementBLL.View_Models.TrainerVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
     

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public bool CreateTrainer(CreateTrainerModelView createTrainer)
        {

            if (createTrainer is null || IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                return false;

            #region Manual
            var trainer = new Trainer
            {
                Name = createTrainer.Name,
                Email = createTrainer.Email,
                Phone = createTrainer.Phone,
                DateOfBirth = createTrainer.DateOfBirth,
                Gender = createTrainer.Gender,
                Address = new Address
                {
                    BuildingNumber = createTrainer.BuildingNumber,
                    Street = createTrainer.Street,
                    City = createTrainer.City
                },
                Specialties = createTrainer.Specialties,


            };
            #endregion
            try
            {
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<TrainerModelView> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            if (trainers is null || !trainers.Any()) return [];



            #region manual
            return  trainers.Select(T => new TrainerModelView
            {
                Id = T.Id,
                Name = T.Name,
                Phone = T.Phone,
                Email = T.Email,
                Specialization = T.Specialties.ToString()
            });
            #endregion
           
        }

        public TrainerModelView? GetTrainerDetails(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerModelView
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialties.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber}, {trainer.Address.Street}, {trainer.Address.City}",
                Gender = trainer.Gender.ToString()
            };

        }

        public TrainerToUpdateModelView? GetTrainerToUpdate(int trainerId)
        {

            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerToUpdateModelView
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };
        }

        public bool DeleteTrainer(int trainerId)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainer = trainerRepo.GetById(trainerId);
            if (trainer is null) return false;

            var hasFutureSession = _unitOfWork.GetRepository<Session>()
                .GetAll(S => S.TrainerId == trainerId && S.StartDate > DateTime.Now).Any();


            if (hasFutureSession) return false;


            try
            {
                trainerRepo.Delete(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateTrainer(int trainerId, TrainerToUpdateModelView trainerToUpdate)
        {

            try
            {
                var trainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerRepo.GetById(trainerId);
                if (trainer is null) return false;
                // valid email state is change or not
                // if change and email exist --> not Unique --> Invalid
                // else Update email
                var emailChange = !(trainer.Email == trainerToUpdate.Email);
                if (emailChange && IsEmailExist(trainerToUpdate.Email))
                    return false;
                else
                    trainer.Email = trainerToUpdate.Email;

                // valid Phone state is change or not
                // if change and Phone exist --> not Unique --> Invalid
                // else update Phone
                var phoneChange = !(trainer.Phone == trainerToUpdate.Phone);
                if (phoneChange && IsPhoneExist(trainerToUpdate.Phone))
                    return false;
                else
                    trainer.Phone = trainerToUpdate.Phone;

                #region Manual
                (trainer.Address.BuildingNumber, trainer.Address.Street, trainer.Address.City, trainer.Specialties) =
                    (trainerToUpdate.BuildingNumber, trainerToUpdate.Street, trainerToUpdate.City, trainerToUpdate.Specialties);
                #endregion



                trainerRepo.Update(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #region Helper Methods

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Phone == phone).Any();
        }
        #endregion
    }
}
