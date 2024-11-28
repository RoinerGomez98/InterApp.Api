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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly Security securityC;
        private readonly IConfiguration _config;
        public StudentController(IStudentService studentService, IConfiguration config, IUserService userService)
        {
            _config = config;
            _studentService = studentService;
            securityC = new(_config);
            _userService = userService;
        }

        [HttpPost]
        [Route("GetStudents")]
        public async Task<GenericResponse<IEnumerable<StudentsDto>>> GetStudents(Generics<StudentsDto> genericRequest)
        {
            return await _studentService.GetStudents(genericRequest.Request!);
        }

        [HttpPost, AllowAnonymous]
        [Route("SaveStudents")]
        public async Task<GenericResponse<int>> SaveStudents(Generics<StudentsDto> genericRequest)
        {
            if (genericRequest.IsNew)
            {
                genericRequest.Request!.Id = 0;

                var verify = await _userService.GetUsers(new UsersDto { Document = genericRequest.Request!.Document, Status = true });
                if (verify.Result!.Any())
                {
                    HttpContext.Response.StatusCode = 400;
                    return new GenericResponse<int> { Message = "Ya se encuentra un usuario registrado con el documento " + genericRequest.Request!.Document, Status = HttpStatusCode.BadRequest };
                }
                genericRequest.Request!.Users!.Add(new Core.Entities.Users
                {
                    Id = 0,
                    Document = genericRequest.Request!.Document!.Trim(),
                    Email = genericRequest.Request!.Email!.Trim(),
                    Name = genericRequest.Request!.Names!.Trim() + " " + genericRequest.Request!.LastName!.Trim(),
                    Status = genericRequest.Request!.Status,
                    TypeUser = 2,
                    Password = securityC.EncryptP(genericRequest.Request!.Document!.Trim())
                });
            }
            var result = await _studentService.SaveStudents(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return new GenericResponse<int> { Message = result.Message, Status = result.Status, Result = result.Result };
        }
    }
}
