using TaskManagement.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseJson<string>> RegisterUserAsync(UserRegisterDto request);
        Task<ResponseJson<string>> AuthenticateUserAsync(UserLoginDto request);
        Task<ResponseJson<List<User>>> GetAllUsersAsync();
        Task<ResponseJson<User>> GetUserByIdAsync(int id);
    }
}
