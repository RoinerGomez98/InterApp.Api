using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface IProfessorService
    {
        Task<GenericResponse<IEnumerable<ProfessorDto>>> GetProfessors(ProfessorDto professorDto);
        Task<GenericResponse<int>> SaveProfessors(ProfessorDto professorDto);
    }
}
