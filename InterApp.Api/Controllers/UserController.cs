using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace InterApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Security securityC;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration config)
        {
            _config = config;
            _userService = userService;
            securityC = new(_config);
        }

        [HttpPost, AllowAnonymous]
        [Route("Autentication")]
        public async Task<GenericResponse<UsersResponseDto>> Autentication(Generics<UsersDto> genericRequest)
        {
            if (string.IsNullOrEmpty(genericRequest.Request!.Document))
            {
                HttpContext.Response.StatusCode = 400;
                return new GenericResponse<UsersResponseDto> { Message = "Digite Documento.", Status = HttpStatusCode.BadRequest };
            }
            if (string.IsNullOrEmpty(genericRequest.Request!.Password))
            {
                HttpContext.Response.StatusCode = 400;
                return new GenericResponse<UsersResponseDto> { Message = "Digite Contraseña.", Status = HttpStatusCode.BadRequest };
            }

            genericRequest.Request!.Password = securityC.EncryptP(genericRequest.Request!.Password!);
            var res = await _userService.GetUsers(genericRequest.Request!);
            UsersResponseDto response = res.Result!.FirstOrDefault()!;
            GenericResponse<UsersResponseDto> tk;
            if (response != null)
            {
                tk = GenerateToken(new UserClaims { Email = response.Email, Document = response.Document, Name = response!.Name, Id = response.Id.ToString() }, response);
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new GenericResponse<UsersResponseDto> { Message = "Usuario no encontrado", Status = HttpStatusCode.BadRequest };
            }

            HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            return new GenericResponse<UsersResponseDto> { Result = tk.Result, Status = HttpStatusCode.OK };
        }

        [HttpPost]
        [Route("GetUsers")]
        public async Task<GenericResponse<IEnumerable<UsersResponseDto>>> GetUsers(Generics<UsersDto> genericRequest)
        {
            return await _userService.GetUsers(genericRequest.Request!);
        }

        [HttpPost]
        [Route("SaveUsers")]
        public async Task<GenericResponse<int>> SaveUsers(Generics<UsersDto> genericRequest)
        {
            if (genericRequest.IsNew)
            {
                genericRequest.Request!.Id = 0;
                genericRequest.Request!.Password = securityC.EncryptP(genericRequest.Request!.Password!);
            }
            var result = await _userService.SaveUsers(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return result;
        }

        private GenericResponse<UsersResponseDto> GenerateToken(UserClaims userClaims, UsersResponseDto userResponse)
        {
            if (userClaims == null)
                return new GenericResponse<UsersResponseDto>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Authentication, userClaims.Id!),
                new Claim(ClaimTypes.Actor, userClaims.Document!),
                new Claim(ClaimTypes.NameIdentifier, userClaims.Name!),
                new Claim(ClaimTypes.Email, userClaims.Email!),
            };

            DateTime expira = DateTime.Now.AddHours(1);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: expira,
              signingCredentials: credentials);

            userResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
            userResponse.Expires = expira;

            return new GenericResponse<UsersResponseDto> { Result = userResponse };
        }
        private class UserClaims
        {
            public string? Document { get; set; }
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? Id { get; set; }
        }
    }
}
