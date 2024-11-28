using InterApp.Common.Dtos;
using InterApp.Common.Utils;

namespace InterApp.Api.Application.Interfaces
{
    public interface IUserService
    {
        Task<GenericResponse<IEnumerable<UsersResponseDto>>> GetUsers(UsersDto usersDto);
        Task<GenericResponse<int>> SaveUsers(UsersDto usersDto);
    }
}
