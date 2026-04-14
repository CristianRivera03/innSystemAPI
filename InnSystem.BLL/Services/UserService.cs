using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Users;
using InnSystem.Model;
using InnSystem.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //Creacion de usuarios
        public async Task<UserDTO> Create(UserCreateDTO model)
        {
            try
            {

                var userModel = _mapper.Map<User>(model);
                userModel.IdUser = Guid.NewGuid();
                //encriptacion
                userModel.PasswordHash = SecurityHelper.HashPassword(model.Password);
                userModel.IdRole = 5;
                userModel.CreatedAt = DateTime.UtcNow;
                userModel.IsActive = true;

                var userCreated = await _userRepository.Create(userModel);

                if (userCreated.IdUser == Guid.Empty)
                    throw new TaskCanceledException("El usuario no se pudo crear");

                return _mapper.Map<UserDTO>(userCreated);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating a new user");
                throw;
            }
        }

        public async Task<SessionDTO> Login(string email, string password)
        {
            try
            {
                var queryUser =  _userRepository.Query(u => u.Email == email && u.IsActive == true);

                var userFound =  await queryUser
                    .Include(u=> u.IdRoleNavigation)
                    .ThenInclude(r => r.IdModules)
                    .FirstOrDefaultAsync();

                if (userFound == null || !SecurityHelper.VerifyPassword(password, userFound.PasswordHash))
                {
                    throw new UnauthorizedAccessException("El usuario no existe o la contraseña es incorrecta");
                }

                return _mapper.Map<SessionDTO>(userFound);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error login");
                throw;
            }
        }

        public Task<bool> ChangeUserRole(UserChangeRoleDTO request)
        {
            throw new NotImplementedException();
        }


        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task<bool> Update(UserDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(UserUpdateDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
