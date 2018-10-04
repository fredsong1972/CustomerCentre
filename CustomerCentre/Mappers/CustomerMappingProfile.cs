using System;
using AutoMapper;
using CustomerCentre.Models;
using CustomerEntity = CustomerCentre.Persistence.Models.Customer;

namespace CustomerCentre.Mappers
{
    public class CustomerMappingProfile : Profile
    {
        /// <summary>
        /// Create auto mapping profile for CustomerCentre.Models.Customer and CustomerCentre.Persistence.Models.Customer
        /// </summary>
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerEntity>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x=>x.DateOfBirth, opt=>opt.MapFrom(src=>src.DateOfBirth))
                .ForMember(x=>x.Modified, opt=>opt.UseValue(DateTimeOffset.UtcNow))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<CustomerEntity, Customer>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
