using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface ISubjectService
    {
        Task<GenericResponse<IEnumerable<SubjectsDto>>> GetSubjects(SubjectsDto subjectsDto);
        Task<GenericResponse<int>> SaveSubjects(SubjectsDto subjectsDto);
    }
}
