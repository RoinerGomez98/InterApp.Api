using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class SubjectService: ISubjectService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public SubjectService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<SubjectsDto>>> GetSubjects(SubjectsDto subjectsDto)
        {
            var cast = _mapper.Map<Subjects>(subjectsDto);
            var result = await _repoInt.GetSubjects(cast);
            GenericResponse<IEnumerable<SubjectsDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<SubjectsDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveSubjects(SubjectsDto subjectsDto)
        {
            var cast = _mapper.Map<Subjects>(subjectsDto);
            int result = await _repoInt.SaveSubjects(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }
}
