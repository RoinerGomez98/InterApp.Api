using AutoMapper;
using InterApp.Api.Application.Interfaces;
using InterApp.Common.Dtos;
using InterApp.Common.Utils;
using InterApp.Core.Entities;
using InterApp.Core.Services;
using System.Net;

namespace InterApp.Api.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly RepoInter _repoInt;
        private readonly IMapper _mapper;
        public UserService(RepoInter repoInt, IMapper mapper)
        {
            _repoInt = repoInt;
            _mapper = mapper;
        }
        public async Task<GenericResponse<IEnumerable<UsersResponseDto>>> GetUsers(UsersDto usersDto)
        {
            var cast = _mapper.Map<Users>(usersDto);
            var result = await _repoInt.GetUsers(cast);
            GenericResponse<IEnumerable<UsersResponseDto>> response = new()
            {
                Result = _mapper.Map<IEnumerable<UsersResponseDto>>(result),
                Status = HttpStatusCode.OK
            };
            return response;

        }
        public async Task<GenericResponse<int>> SaveUsers(UsersDto usersDto)
        {
            var cast = _mapper.Map<Users>(usersDto);
            int result = await _repoInt.SaveUsers(cast);
            return new()
            {
                Result = result,
                Message = result > 0 ? "Se guardó correctamente información" : "No se pudo guardar información",
                Status = result > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest
            };
        }
    }
}
