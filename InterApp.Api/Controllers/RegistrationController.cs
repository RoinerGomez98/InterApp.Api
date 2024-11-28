using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController(IRegistrationService registrationService) : ControllerBase
    {
        private readonly IRegistrationService _registrationService = registrationService;

        [HttpPost]
        [Route("GetRegistrations")]
        public async Task<GenericResponse<IEnumerable<RegistrationsDto>>> GetRegistrations(Generics<RegistrationsDto> genericRequest)
        {
            return await _registrationService.GetRegistrations(genericRequest.Request!);
        }

        [HttpPost]
        [Route("SaveRegistrations")]
        public async Task<GenericResponse<int>> SaveRegistrations(Generics<IEnumerable<RegistrationsDto>> genericRequest)
        {
            foreach (var item in genericRequest.Request!)
            {
                item.Status = true;
                var exist = await _registrationService.GetRegistrations(new RegistrationsDto { ProfessorId = item.ProfessorId, StudentId = item.StudentId });
                if (exist.Result!.Any())
                {
                    HttpContext.Response.StatusCode = 400;
                    return new GenericResponse<int> { Message = $"El estudiante ya tiene clases con el profesor {exist.Result!.FirstOrDefault()!.Professor} en otra materia.", Status = HttpStatusCode.BadRequest, Result = 0 };
                }
            }
            var result = await _registrationService.SaveRegistrations(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return new GenericResponse<int> { Message = result.Message, Status = result.Status, Result = result.Result };
        }
    }
}
