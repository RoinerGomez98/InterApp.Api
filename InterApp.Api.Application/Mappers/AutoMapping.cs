using AutoMapper;
using InterApp.Common.Dtos;
using InterApp.Core.Entities;

namespace InterApp.Application.Mappers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Professor, ProfessorDto>().ReverseMap();
            CreateMap<ProfessorSubject, ProfessorSubjectDto>().ReverseMap();
            CreateMap<Registrations, RegistrationsDto>().ReverseMap();
            CreateMap<Students, StudentsDto>().ReverseMap();
            CreateMap<Subjects, SubjectsDto>().ReverseMap();
            CreateMap<TypeDocuments, TypeDocumentsDto>().ReverseMap();
            CreateMap<Users, UsersDto>().ReverseMap();
            CreateMap<Users, UsersResponseDto>().ReverseMap();
        }
    }
}
