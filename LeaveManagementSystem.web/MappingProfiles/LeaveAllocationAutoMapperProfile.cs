using AutoMapper;
using LeaveManagementSystem.web.Models.LeaveAllocations;
using LeaveManagementSystem.web.Models.LeaveAllocations.Periods;
using LeaveManagementSystem.web.Models.LeaveTypes;

namespace LeaveManagementSystem.web.MappingProfiles

{
    public class LeaveAllocationAutoMapperProfile : Profile
    {
        public LeaveAllocationAutoMapperProfile()
        {
            CreateMap<Data.LeaveAllocation, LeaveAllocationsVM>();
            CreateMap<Data.LeaveAllocation, LeaveAllocationEditVM>();
            CreateMap<Data.ApplicationUser, EmployeeListVM>().ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Data.Period, PeriodVM>();
              
        }
    }
}
