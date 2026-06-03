using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Common;
using InnSystem.DTO.Roles;
using InnSystem.DTO.Rooms;
using InnSystem.DTO.Users;
using InnSystem.Model;
using InnSystem.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using InnSystem.Utility.Interfaces;

namespace InnSystem.BLL.Services
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IDataProtector _dataProtector;
        private readonly IEmailService _emailService;

        public UserService(
            IGenericRepository<User> userRepository, 
            IMapper mapper, 
            ILogger<UserService> logger,
            IDataProtectionProvider dataProtectionProvider,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _dataProtector = dataProtectionProvider.CreateProtector("PasswordReset");
            _emailService = emailService;
        }

        //Listar usuarios
        public async Task<List<UserDTO>> GetAllAsync()
        {
            try
            {
                var listUsers = await _userRepository.Query(r => r.DeletedAt == null).ToListAsync();

                return _mapper.Map<List<UserDTO>>(listUsers);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios");
                throw;
            }
        }

        // Listar usuarios paginados y filtrados
        public async Task<PagedResultDTO<UserDTO>> GetPaginatedAsync(string search, string roleName, int page, int limit)
        {
            try
            {
                var query = _userRepository.Query(u => u.DeletedAt == null);

                if (!string.IsNullOrEmpty(search))
                {
                    var s = search.ToLower();
                    query = query.Where(u => 
                        u.FirstName.ToLower().Contains(s) || 
                        u.LastName.ToLower().Contains(s) || 
                        u.Email.ToLower().Contains(s) || 
                        (u.DocumentId != null && u.DocumentId.ToLower().Contains(s)));
                }

                if (!string.IsNullOrEmpty(roleName))
                {
                    query = query.Where(u => u.IdRoleNavigation.RoleName == roleName);
                }

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalItems / (double)limit);

                var users = await query
                    .Include(u => u.IdRoleNavigation)
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                return new PagedResultDTO<UserDTO>
                {
                    Items = _mapper.Map<List<UserDTO>>(users),
                    TotalItems = totalItems,
                    CurrentPage = page,
                    TotalPages = totalPages > 0 ? totalPages : 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios paginados");
                throw;
            }
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
                userModel.IdRole = model.IdRole > 0 ? model.IdRole : 5; // 5 = Client
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



        public Task<bool> Update(UserDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(Guid idUser, UserUpdateDTO request)
        {
            try
            {
                var userFound = await _userRepository.Query(u => u.IdUser == idUser).FirstOrDefaultAsync();

                if (userFound == null)
                    throw new TaskCanceledException("El usuario no existe");

                userFound.IdRole = request.IdRole;
                userFound.FirstName = request.FirstName;
                userFound.LastName = request.LastName;
                userFound.Email = request.Email;
                userFound.Phone = request.Phone;
                userFound.DocumentId = request.DocumentId;
                userFound.IsActive = request.IsActive;

                bool response = await _userRepository.Update(userFound);

                if (!response)
                    throw new TaskCanceledException("No se pudo editar el usuario");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                throw;
            }
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userRepository.Query(u => u.Email == email && u.IsActive == true).FirstOrDefaultAsync();
                if (user == null)
                {
                    // No arrojar error explícito para no revelar qué correos existen en el sistema (por seguridad).
                    return true;
                }

                // Generar token: Formato "IdUser|FechaExpiracionTicks"
                var expiryTime = DateTime.UtcNow.AddHours(1).Ticks.ToString();
                var rawToken = $"{user.IdUser}|{expiryTime}";
                var protectedToken = _dataProtector.Protect(rawToken);

                // Codificar para URL (Base64 puede tener +, / que rompen la URL)
                // O mejor aún, UrlEncode
                var urlSafeToken = Uri.EscapeDataString(protectedToken);

                var resetLink = $"http://localhost:4200/reset-password?token={urlSafeToken}&email={Uri.EscapeDataString(email)}";

                var body = $@"
                    <h2>Recuperación de Contraseña</h2>
                    <p>Hola {user.FirstName},</p>
                    <p>Hemos recibido una solicitud para restablecer tu contraseña en InnSystem Hotel.</p>
                    <p>Si fuiste tú, haz clic en el siguiente enlace para crear una nueva contraseña. El enlace es válido por 1 hora:</p>
                    <a href='{resetLink}' style='display:inline-block; padding:10px 20px; background-color:#0f62fe; color:#ffffff; text-decoration:none; border-radius:4px;'>Restablecer Contraseña</a>
                    <br/><br/>
                    <p>Si no solicitaste esto, puedes ignorar este correo de forma segura.</p>
                    <p>Gracias,<br/>El equipo de InnSystem</p>";

                await _emailService.SendEmailAsync(user.Email, "Restablece tu contraseña - InnSystem", body, null, null);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ForgotPasswordAsync");
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO request)
        {
            try
            {
                var user = await _userRepository.Query(u => u.Email == request.Email && u.IsActive == true).FirstOrDefaultAsync();
                if (user == null)
                    throw new UnauthorizedAccessException("Usuario inválido o inactivo.");

                string rawToken;
                try
                {
                    rawToken = _dataProtector.Unprotect(request.Token);
                }
                catch
                {
                    throw new UnauthorizedAccessException("El token es inválido o está corrupto.");
                }

                var parts = rawToken.Split('|');
                if (parts.Length != 2)
                    throw new UnauthorizedAccessException("Formato de token inválido.");

                if (parts[0] != user.IdUser.ToString())
                    throw new UnauthorizedAccessException("El token no pertenece a este usuario.");

                if (!long.TryParse(parts[1], out long expiryTicks) || new DateTime(expiryTicks, DateTimeKind.Utc) < DateTime.UtcNow)
                {
                    throw new UnauthorizedAccessException("El enlace de restablecimiento ha expirado.");
                }

                user.PasswordHash = SecurityHelper.HashPassword(request.NewPassword);
                return await _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ResetPasswordAsync");
                throw;
            }
        }
    }
}
