using AutoMapper;
using GymManagementBLL.View_Models.MemberVM;
using GymManagementBLL.View_Models.PlanVM;
using GymManagementBLL.View_Models.SessionVM;
using GymManagementBLL.View_Models.TrainerVM;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.View_Models.SessionVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
          .ForMember(dest => dest.TrainerName, options => options.MapFrom(scr => scr.Trainer.Name))
          .ForMember(dest => dest.CategoryName, option => option.MapFrom(scr => scr.Category.CategoryName))
          .ForMember(dest => dest.AvailableSlots, option => option.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session ,UpdateSessionViewModel>().ReverseMap();

        }


      

        
    }
}
