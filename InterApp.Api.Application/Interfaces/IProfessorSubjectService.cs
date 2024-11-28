using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface IProfessorSubjectService
    {
        Task<GenericResponse<IEnumerable<ProfessorSubjectDto>>> GetProfessorSubjects(ProfessorSubjectDto professorSubjectDto, int StudentId = 0);
        Task<GenericResponse<int>> SaveProfessorSubjects(IEnumerable<ProfessorSubjectDto> professorSubjectDto);
    }
}
