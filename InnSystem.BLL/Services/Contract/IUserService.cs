using InnSystem.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<SessionDTO> Login(string email, string password);
        Task<UserDTO> Create(UserCreateDTO model);
        Task<bool> Update(UserDTO model);
        Task<bool> Delete(Guid id);
        Task<bool> ChangeUserRole(UserChangeRoleDTO request);
        Task<bool> UpdateUser(Guid idUser, UserUpdateDTO request);
    }
}
