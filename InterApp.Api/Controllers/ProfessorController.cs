using InterApp.Api.Application.Implementations;
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
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _professorService;
        private readonly IUserService _userService;
        private readonly Security securityC;
        private readonly IConfiguration _config;
        public ProfessorController(IProfessorService professorService, IConfiguration config, IUserService userService)
        {
            _config = config;
            _professorService = professorService;
            securityC = new(_config);
            _userService = userService;
        }

        [HttpPost]
        [Route("GetProfessors")]
        public async Task<GenericResponse<IEnumerable<ProfessorDto>>> GetProfessors(Generics<ProfessorDto> genericRequest)
        {
            return await _professorService.GetProfessors(genericRequest.Request!);
        }

        [HttpPost, AllowAnonymous]
        [Route("SaveProfessors")]
        public async Task<GenericResponse<int>> SaveProfessors(Generics<ProfessorDto> genericRequest)
        {
            if (genericRequest.IsNew)
            {
                genericRequest.Request!.Id = 0;
                genericRequest.Request!.Status = true;

                var verify = await _userService.GetUsers(new UsersDto { Document = genericRequest.Request!.Document, Status = true });
                if (verify.Result!.Any())
                {
                    HttpContext.Response.StatusCode = 400;
                    return new GenericResponse<int> { Message = "Ya se encuentra un usuario registrado con el documento " + genericRequest.Request!.Document , Status = HttpStatusCode.BadRequest };
                }
                genericRequest.Request!.Users!.Add(new Core.Entities.Users
                {
                    Id = 0,
                    Document = genericRequest.Request!.Document,
                    Email = genericRequest.Request!.Email,
                    Name = genericRequest.Request!.Names + " " + genericRequest.Request!.LastName,
                    Status = true,
                    TypeUser = 3,
                    Password = securityC.EncryptP(genericRequest.Request!.Document!)
                });
            }
            var result = await _professorService.SaveProfessors(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return new GenericResponse<int> { Message = result.Message, Status = result.Status, Result = result.Result };
        }
    }
}
