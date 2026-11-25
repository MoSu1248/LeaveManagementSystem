using AutoMapper;
using LeaveManagementSystem.web.Models.LeaveRequests;
using LeaveManagementSystem.web.Models.LeaveAllocations;
using LeaveManagementSystem.web.Models.LeaveAllocations.Periods;

namespace LeaveManagementSystem.web.MappingProfiles

{
    public class LeaveRequestAutoMapperProfile : Profile
    {
        public LeaveRequestAutoMapperProfile()
        {
            CreateMap<LeaveRequestVM, Data.LeaveRequest>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveRequestStatus, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveRequestStatusId, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ForMember(dest => dest.Reviewer, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewerId, opt => opt.Ignore());

            CreateMap<Data.LeaveRequest, EmployeeLeaveRequestListVM>()
                .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src =>
                    src.LeaveType == null ? string.Empty : src.LeaveType.Name))
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src =>
                    src.EndDate.DayNumber - src.StartDate.DayNumber))
                .ForMember(dest => dest.LeaveRequestStatus, opt => opt.MapFrom(src =>
                    (Data.LeaveRequestStatusCode)src.LeaveRequestStatusId));

            //CreateMap<Data.LeaveRequest, ReviewLeaveRequestVM>()
            //    .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.EmployeeId))
            //    .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src =>
            //        src.LeaveType == null ? string.Empty : src.LeaveType.Name))
            //    .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src =>
            //        src.EndDate.DayNumber - src.StartDate.DayNumber))
            //    .ForMember(dest => dest.LeaveRequestId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
