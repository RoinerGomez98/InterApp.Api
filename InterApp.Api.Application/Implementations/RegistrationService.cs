using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class RegistrationService: IRegistrationService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public RegistrationService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }


        public async Task<GenericResponse<IEnumerable<RegistrationsDto>>> GetRegistrations(RegistrationsDto registrationsDto)
        {
            var cast = _mapper.Map<Registrations>(registrationsDto);
            var result = await _repoInt.GetRegistrations(cast);
            GenericResponse<IEnumerable<RegistrationsDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<RegistrationsDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveRegistrations(IEnumerable<RegistrationsDto> registrationsDto)
        {
            var cast = _mapper.Map<IEnumerable<Registrations>>(registrationsDto);
            int result = await _repoInt.SaveRegistrations(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }

    }
}
