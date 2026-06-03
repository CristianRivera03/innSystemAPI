using InnSystem.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InnSystem.DTO.Common;

namespace InnSystem.BLL.Services.Contract
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<PagedResultDTO<UserDTO>> GetPaginatedAsync(string search, string roleName, int page, int limit);
        Task<SessionDTO> Login(string email, string password);
        Task<UserDTO> Create(UserCreateDTO model);
        Task<bool> Update(UserDTO model);
        Task<bool> Delete(Guid id);
        Task<bool> ChangeUserRole(UserChangeRoleDTO request);
        Task<bool> UpdateUser(Guid idUser, UserUpdateDTO request);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO request);
    }
}
