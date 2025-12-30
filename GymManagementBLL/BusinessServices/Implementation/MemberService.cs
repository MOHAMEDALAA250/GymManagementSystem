using GymManagementBLL.BusinessServices.Interface;
using GymManagementBLL.View_Models.MemberVM;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MemberService : IMemeberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            // Email not exist

            //var members = _memberRepository.GetAll();
            //if(members.Any())
            //{
            //    foreach(var member in members)
            //    {
            //        if (member.Email == createMember.Email)
            //            return false;
            //    }
            //}


            // Email and phone not exist

            if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;




            //CreateMemberViewModel => Member

            #region manual
            var member = new Member
            {
                Name = createMember.Name,
                Email = createMember.Email,
                Phone = createMember.Phone,
                Gender = createMember.Gender,
                DateOfBirth = createMember.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = createMember.BuildingNumber,
                    City = createMember.City,
                    Street = createMember.Street
                },
                HealthRecord = new HealthRecord
                {
                    Height = createMember.HealthRecord.Height,
                    Weight = createMember.HealthRecord.Weight,
                    BloodType = createMember.HealthRecord.BloodType,
                    Note = createMember.HealthRecord.Note
                }
            };
            #endregion

            //Create Memeber in Database
           
          _unitOfWork.GetRepository<Member>().Add(member);
           return _unitOfWork.SaveChanges()>0;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

            if (members is null || !members.Any()) return [];

            #region Manual Mapping First way

            //var listOfMemberViewModels = new List<MemberViewModel>();

            //foreach (var member in members)
            //{

            //    var memberViewModels = new MemberViewModel
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Phone = member.Phone,
            //        Photo = member.Photo,
            //        Email = member.Email,
            //        Gender = member.Gender.ToString(),
            //    };

            //    listOfMemberViewModels.Add(memberViewModels);
            //}
            //return listOfMemberViewModels;
            #endregion

            #region Manual Mapping projection

            var memberViewModel = members.Select(M => new MemberViewModel
            {
                Id = M.Id,
                Name = M.Name,
                Email = M.Email,
                Phone = M.Phone,
                Photo = M.Photo,
                Gender = M.Gender.ToString()
            });

            return memberViewModel;
            #endregion
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            var memberViewModel = new MemberViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber}-{member.Address.Street}-{member.Address.City}",
                Photo = member.Photo,
            };

            var memberShip = _unitOfWork.GetRepository<MemberShip>()
                .GetAll(X => X.MemberId == memberId&&X.Status=="Active")
                .FirstOrDefault();

            if (memberShip is not null)
            {
                memberViewModel.MembershipStartDate= memberShip.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate= memberShip.EndDate.ToShortDateString();

                var plan = _unitOfWork.GetRepository<Plan>().GetById(memberShip.PlanId);

                memberViewModel.PlanName = plan?.Name;
            }

            return memberViewModel;
        }

        public MemberToUpdateViewModel? GetMemberDetailsToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null) return null;

            return new MemberToUpdateViewModel
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };
        }

        public HealthRecordViewModel? GetMemberHealthDetails(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);

            if (memberHealthRecord is null) return null;

            return new HealthRecordViewModel
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note
            };
        }

        public bool RemoveMember(int memberId)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();

                var member = memberRepo.GetById(memberId);

                if (member is null) return false;

                var memberSessionsIds = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == memberId)
                    .Select(X => X.SessionId);

                var hasFutureSessions = _unitOfWork.GetRepository<Session>()
                    .GetAll(S => memberSessionsIds.Contains(S.Id) && S.StartDate > DateTime.Now).Any();

                if (hasFutureSessions) return false;


                var membershipRepo = _unitOfWork.GetRepository<MemberShip>();

                var memberShips = membershipRepo
                    .GetAll(X => X.MemberId == memberId);

                if (memberShips.Any())
                {
                    foreach (var memberShip in memberShips)
                    {
                        membershipRepo.Delete(memberShip);
                    }
                }

                 memberRepo.Delete(member) ;

                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateMember(int memberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {

                var memberRepo = _unitOfWork.GetRepository<Member>();

                if (IsEmailExist(memberToUpdate.Email) || IsPhoneExist(memberToUpdate.Phone)) return false;

                var member = memberRepo.GetById(memberId);
                if (member is null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.City = memberToUpdate.City;
                member.Address.Street = memberToUpdate.Street;
                member.UpdatedAt = DateTime.Now;

                 memberRepo.Update(member) ;

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }


        #region HelperMethod

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }



        #endregion

    }

}
