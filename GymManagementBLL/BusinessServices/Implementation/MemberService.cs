using GymManagementBLL.BusinessServices.Interface;
using GymManagementBLL.View_Models.MemberVM;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MemberService : IMemeberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
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

            var EmailExist = _memberRepository.GetAll(M => M.Email == createMember.Email).Any();
            var PhoneExist = _memberRepository.GetAll(M => M.Phone == createMember.Phone).Any();

            // Email and phone not exist

            if (EmailExist || PhoneExist) return false;



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
           
           return _memberRepository.Add(member) > 0;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _memberRepository.GetAll();

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
            throw new NotImplementedException();
        }
    }

}
