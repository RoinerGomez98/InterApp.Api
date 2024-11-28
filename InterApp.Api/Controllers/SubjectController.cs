using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController(ISubjectService subjectService) : ControllerBase
    {
        private readonly ISubjectService _subjectService = subjectService;

        [HttpPost]
        [Route("GetSubjects")]
        public async Task<GenericResponse<IEnumerable<SubjectsDto>>> GetSubjects(Generics<SubjectsDto> genericRequest)
        {
            return await _subjectService.GetSubjects(genericRequest.Request!);
        }

        [HttpPost]
        [Route("SaveSubjects")]
        public async Task<GenericResponse<int>> SaveSubjects(Generics<SubjectsDto> genericRequest)
        {
            if (genericRequest.IsNew)
            {
                genericRequest.Request!.Id = 0;
            }
            genericRequest.Request!.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var result = await _subjectService.SaveSubjects(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return new GenericResponse<int> { Message = result.Message, Status = result.Status, Result = result.Result };
        }


    }
}
