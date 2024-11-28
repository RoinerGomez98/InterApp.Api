using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<GenericResponse<IEnumerable<RegistrationsDto>>> GetRegistrations(RegistrationsDto registrationsDto);
        Task<GenericResponse<int>> SaveRegistrations(IEnumerable<RegistrationsDto> registrationsDto);
    }
}
