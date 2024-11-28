using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class ProfessorSubjectService : IProfessorSubjectService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public ProfessorSubjectService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<ProfessorSubjectDto>>> GetProfessorSubjects(ProfessorSubjectDto professorSubjectDto, int StudentId = 0)
        {
            var cast = _mapper.Map<ProfessorSubject>(professorSubjectDto);
            var result = await _repoInt.GetProfessorSubjects(cast, StudentId);
            GenericResponse<IEnumerable<ProfessorSubjectDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<ProfessorSubjectDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveProfessorSubjects(IEnumerable<ProfessorSubjectDto> professorSubjectDto)
        {
            var cast = _mapper.Map<IEnumerable<ProfessorSubject>>(professorSubjectDto);
            int result = await _repoInt.SaveProfessorSubjects(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }
}
