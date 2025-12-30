using AutoMapper;
using GymManagementBLL.BusinessServices.Interface;
using GymManagementBLL.View_Models.SessionVM;
using GymManagementDAL.Entities;
using GymManagementDAL.UnitOfWork;
using GymManagementSystemBLL.View_Models.SessionVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                //check if trainer Exist 
                if (!IsTrainerExist(createSession.TrainerId))
                    return false;
                //check if category exist
                if (!IsCategoryExist(createSession.CategoryId))
                    return false;
                //check if  start date before enddate
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate))
                    return false;

                if (createSession.Capacity > 25 || createSession.Capacity < 0)
                    return false;

                var sessionToCreate = _mapper.Map<CreateSessionViewModel, Session>(createSession);

                _unitOfWork.SessionRepository.Add(sessionToCreate);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = sessionRepo.GetAllWithCategoryAndTrainer();

            if (!sessions.Any())
                return [];

            #region Manual
            //return sessions.Select(session => new SessionViewModel
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate, 
            //    TrainerName = session.Trainer.Name,
            //    CategoryName = session.Category.CategoryName,
            //    AvailableSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id)

            //}); 
            #endregion
            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
                session.AvailableSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);
            return mappedSessions;
        }

        public SessionViewModel? GetSessionDetails(int sessionId)
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var session = sessionRepo.GetByIdWithTrainerAndCategory(sessionId);

            if (session is null)
                return null;

            #region Manual Mapping
            //return new SessionViewModel
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    TrainerName = session.Trainer.Name,
            //    CategoryName = session.Category.CategoryName,
            //    AvailableSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id)

            //}; 
            #endregion
            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);

            return mappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvalibleToUpdate(session!))
                return null;

            return _mapper.Map<UpdateSessionViewModel>(session);

        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updateSession)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvalibleToUpdate(session!))
                    return false;

                //check if category exist
                if (!IsTrainerExist(updateSession.TrainerId))
                    return false;
                //check if  start date before enddate
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate))
                    return false;

                _mapper.Map(updateSession, session);

                session!.UpdatedAt = DateTime.Now;

                _unitOfWork.SessionRepository.Update(session!);


                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool DeleteSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvalibleForRemoving(session!))
                    return false;

                _unitOfWork.SessionRepository.Delete(session!);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region Helpeer methods

        private bool IsTrainerExist(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        private bool IsCategoryExist(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        private bool IsDateTimeValid(DateTime startDate, DateTime endData)
        {
            return startDate < endData && DateTime.Now < startDate;
        }

        private bool IsSessionAvalibleToUpdate(Session session)
        {
            if (session is null) return false;

            // if session Completed - no update avalible

            if (session.EndDate < DateTime.Now)
                return false;

            if (session.StartDate <= DateTime.Now)
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;

        }
        private bool IsSessionAvalibleForRemoving(Session session)
        {
            if (session is null) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;
            if (session.StartDate > DateTime.Now)
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;

        }



        #endregion
    }
}
