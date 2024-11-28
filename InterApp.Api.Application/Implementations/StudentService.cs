using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class StudentService: IStudentService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public StudentService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<StudentsDto>>> GetStudents(StudentsDto studentsDto)
        {
            var cast = _mapper.Map<Students>(studentsDto);
            var result = await _repoInt.GetStudents(cast);
            GenericResponse<IEnumerable<StudentsDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<StudentsDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveStudents(StudentsDto studentsDto)
        {
            var cast = _mapper.Map<Students>(studentsDto);
            int result = await _repoInt.SaveStudents(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }
}
