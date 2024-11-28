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
    public class ProfessorSubjectController(IProfessorSubjectService professorSubjectService) : ControllerBase
    {
        private readonly IProfessorSubjectService _professorSubjectService = professorSubjectService;

        [HttpPost]
        [Route("GetProfessorSubjects")]
        public async Task<GenericResponse<IEnumerable<ProfessorSubjectDto>>> GetProfessorSubjects(Generics<ProfessorSubjectDto> genericRequest)
        {
            return await _professorSubjectService.GetProfessorSubjects(genericRequest.Request!, genericRequest.Request!.StudentId!.Value);
        }

        [HttpPost]
        [Route("SaveProfessorSubjects")]
        public async Task<GenericResponse<int>> SaveProfessorSubjects(Generics<IEnumerable<ProfessorSubjectDto>> genericRequest)
        {

            foreach (var item in genericRequest.Request!)
            {
                item.Status = true;
                item.Id = 0;
                item.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId")!);
            }

            var result = await _professorSubjectService.SaveProfessorSubjects(genericRequest.Request!);
            HttpContext.Response.StatusCode = (int)result.Status;
            return new GenericResponse<int> { Message = result.Message, Status = result.Status, Result = result.Result };
        }
    }
}
