using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class ProfessorService : IProfessorService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public ProfessorService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }
        public async Task<GenericResponse<IEnumerable<ProfessorDto>>> GetProfessors(ProfessorDto professorDto)
        {
            var cast = _mapper.Map<Professor>(professorDto);
            var result = await _repoInt.GetProfessors(cast);
            GenericResponse<IEnumerable<ProfessorDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<ProfessorDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveProfessors(ProfessorDto professorDto)
        {
            var cast = _mapper.Map<Professor>(professorDto);
            int result = await _repoInt.SaveProfessors(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }
}
