using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface IStudentService
    {
        Task<GenericResponse<IEnumerable<StudentsDto>>> GetStudents(StudentsDto studentsDto);
        Task<GenericResponse<int>> SaveStudents(StudentsDto studentsDto);
    }
}
