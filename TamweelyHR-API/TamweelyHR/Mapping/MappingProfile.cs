using AutoMapper;
using Core.DTOs.Department;
using Core.DTOs.Employees;
using Core.DTOs.Jobs;
using Core.Entities;

namespace API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Employee

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.JobName,
                    opt => opt.MapFrom(src => src.Job.Name));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();


            // Department


            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();


            // Job

            CreateMap<Job, JobDto>();
            CreateMap<CreateJobDto, Job>();
            CreateMap<UpdateJobDto, Job>();
        }
    }
}
