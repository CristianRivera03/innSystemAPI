using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Bookings;
using InnSystem.DTO.Rooms;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace InnSystem.BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IGenericRepository<Room> roomRepository, IMapper mapper, ILogger<RoomService> logger)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomDTO> CreateAsync(RoomCreateDTO model)
        {
            try
            {
                var roomToCreate = _mapper.Map<Room>(model);

                roomToCreate.CreatedAt = DateTime.UtcNow;

                var roomCreated = await _roomRepository.Create(roomToCreate);

                if (roomCreated.IdRoom == 0)
                    throw new TaskCanceledException("The room cannot be created");

                var createdWithNav = await _roomRepository.Query(r => r.IdRoom == roomCreated.IdRoom)
                    .Include(r => r.IdRoomTypeNavigation)
                    .Include(r => r.IdStatusNavigation)
                    .FirstOrDefaultAsync();

                return _mapper.Map<RoomDTO>(createdWithNav);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating the room");
                throw;
            }
        }

        public async Task<List<RoomDTO>> GetAllAsync()
        {
            try
            {
                var listRoom = await _roomRepository.Query(r => r.DeletedAt == null)
                    .Include(r => r.IdRoomTypeNavigation)
                    .Include(r => r.IdStatusNavigation)
                    .ToListAsync();

                return _mapper.Map<List<RoomDTO>>(listRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting the rooms");
                throw;
            }
        }

        public async Task<RoomDTO> GetByIdAsync(int id)
        {
            try
            {

                var room = await _roomRepository.Query(r => r.IdRoom == id && r.DeletedAt == null)
                    .Include(r => r.IdRoomTypeNavigation)
                    .Include(r => r.IdStatusNavigation)
                    .FirstOrDefaultAsync();

                if (room == null)
                    return null; 

                return _mapper.Map<RoomDTO>(room);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting room with ID {id}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id ,RoomUpdateDTO model )
        {
            try
            {
                var existingRoom = await _roomRepository.Get(r => r.IdRoom == id && r.DeletedAt == null);

                if (existingRoom == null)
                    return false;

                _mapper.Map(model, existingRoom);

                return await _roomRepository.Update(existingRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating room{id}");
                throw;
            }
        }

        public async Task<bool> SoftDelete(int id)
        {

            try
            {

                var room = await _roomRepository.Get(r => r.IdRoom == id  && r.DeletedAt == null);

                if (room == null)
                    throw new TaskCanceledException("La habitacion no existe");

                room.IdStatus = 3; // 3 = Inactive
                room.DeletedAt = DateTime.UtcNow;

                bool response = await _roomRepository.SoftDelete(room);

                if (!response)
                    throw new TaskCanceledException("El post no se pudo eliminar ");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post with ID: {UserId}", id);
                throw;
            }
        }

        
        public async Task<bool> HardDelete(int id)
        {
            try
            {
                var room = await _roomRepository.Get(r => r.IdRoom == id);
                if (room == null)
                    return false;

                return await _roomRepository.RemoveRange(new[] { room });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error hard-deleting room {id}");
                throw;
            }
        }

        public async Task<bool> ChangeOperationalStatusAsync(int id, int statusId)
        {
            try
            {
                var room = await _roomRepository.Get(r => r.IdRoom == id);

                if (room == null) return false;

                room.IdStatus = statusId; 
                return await _roomRepository.Update(room);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error changing status for room {id}");
                throw;
            }
        }

        public async Task<List<RoomDTO>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int guestsCount)
        {
            try
            {
                if(checkIn.Date >= checkOut.Date)
                    throw new ArgumentException("La fecha de salida debe ser posterior a la fecha de entrada.");

                if (checkIn.Date < DateTime.UtcNow.Date)
                    throw new ArgumentException("No se pueden buscar fechas en el pasado.");

                var checkInDate = DateOnly.FromDateTime(checkIn);
                var checkOutDate = DateOnly.FromDateTime(checkOut);

                var query = _roomRepository.Query(room =>
                    // Regla A: La habitación debe existir, estar activa y caber la gente
                    room.DeletedAt == null &&
                    room.IdStatus == 1 && // Active
                    room.IdRoomTypeNavigation != null && room.IdRoomTypeNavigation.GuestCapacity >= guestsCount &&

                    // Regla B: Anti-Overbooking (Que NO tenga reservas cruzadas)
                    !room.Bookings.Any(booking =>
                        booking.DeletedAt == null &&
                        (booking.IdStatus == 1 || booking.IdStatus == 2) &&

                        // Lógica universal de traslape de fechas
                        booking.CheckIn < checkOutDate &&
                        booking.CheckOut > checkInDate
                        )
                    )
                    .Include(r => r.IdRoomTypeNavigation)
                    .Include(r => r.IdStatusNavigation);

                var rooms = await query.ToListAsync();
                return _mapper.Map<List<RoomDTO>>(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available rooms");
                throw;
            }
        }

    }
}
