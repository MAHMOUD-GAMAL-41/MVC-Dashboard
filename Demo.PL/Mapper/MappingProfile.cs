using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;

namespace Demo.PL.Mapper
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
            CreateMap<Department,DepartmentViewModel>().ReverseMap();
        }
    }
}
