using InterApp.Api.Application.Implementations;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace InterApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenericsController(IGenericsService genericsService) : ControllerBase
    {
        private readonly IGenericsService _genericsService = genericsService;

        [HttpPost, AllowAnonymous]
        [Route("GetTypeDocuments")]
        public async Task<GenericResponse<IEnumerable<TypeDocumentsDto>>> GetTypeDocuments(Generics<TypeDocumentsDto> genericRequest)
        {
            return await _genericsService.GetTypeDocuments(genericRequest.Request!);
        }




    }
}
