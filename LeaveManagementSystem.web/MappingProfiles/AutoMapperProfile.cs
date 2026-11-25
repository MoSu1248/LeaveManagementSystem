using AutoMapper;
using LeaveManagementSystem.web.Models.LeaveTypes;

namespace LeaveManagementSystem.web.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.LeaveType, Models.LeaveTypes.LeaveTypeReadOnlyVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays));
            
         
            CreateMap<Data.LeaveType, Models.LeaveTypes.LeaveTypeEditVM>()
                .ForMember(dest => dest.Days,opt => opt.MapFrom(src => src.NumberOfDays));

            CreateMap<Data.LeaveType, Models.LeaveTypes.LeaveTypeDeleteVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays));
            
            CreateMap<Models.LeaveTypes.LeaveTypeCreateVM, Data.LeaveType>()
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveAllocations, opt => opt.Ignore());

            CreateMap<Models.LeaveTypes.LeaveTypeEditVM, Data.LeaveType>()
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days))
                .ForMember(dest => dest.LeaveAllocations, opt => opt.Ignore());

            CreateMap<Models.LeaveTypes.LeaveTypeDeleteVM, Data.LeaveType>()
                .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.Days))
                .ForMember(dest => dest.LeaveAllocations, opt => opt.Ignore());
        }
    }
}
