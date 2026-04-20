using AutoMapper;
using Microsoft.EntityFrameworkCore;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Bookings;
using InnSystem.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services
{
    public class BookingService : IBookingService
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookingDTO>> GetAllAsync()
        {
            try
            {
                var queryBooking = _bookingRepository.Query(b => b.DeletedAt == null);
                var listBooking = await queryBooking
                    .Include(b => b.IdUserNavigation)
                    .Include(b => b.IdStatusNavigation)
                    .ToListAsync();

                return _mapper.Map<List<BookingDTO>>(listBooking);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting the bookings");
                throw;
            }
        }

        public async Task<BookingDTO?> GetByIdAsync(Guid id)
        {
            try
            {
                var booking = await _bookingRepository.Query(b => b.IdBooking == id && b.DeletedAt == null)
                    .Include(b => b.IdUserNavigation)
                    .Include(b => b.IdStatusNavigation)
                    .FirstOrDefaultAsync();

                return booking == null ? null : _mapper.Map<BookingDTO>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking {Id}", id);
                throw;
            }
        }

        public async Task<BookingDTO> Create(BookingCreateDTO model)
        {
            try
            {

                var checkInUtc = DateTime.SpecifyKind(model.CheckIn, DateTimeKind.Utc);
                var checkOutUtc = DateTime.SpecifyKind(model.CheckOut, DateTimeKind.Utc);

                var newBookingId = await _bookingRepository.CreateBookingAsync(
                    model.IdUser,
                    model.IdRoom,
                    checkInUtc,
                    checkOutUtc,
                    model.GuestsCount
                );

                // El SP devuelve el Guid de la nueva reserva; recuperamos el DTO completo.
                var bookingDto = await GetByIdAsync(newBookingId);

                if (bookingDto == null)
                    throw new Exception("La reserva fue creada pero no se pudo recuperar.");

                return bookingDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la reserva mediante el stored procedure");
                throw;
            }
        }

        public async Task<bool> HardDelete(Guid id)
        {
            try
            {
                var booking = await _bookingRepository.GetById(id);
                if (booking == null) return false;
                
                return await _bookingRepository.HardDelete(booking);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error hard deleting booking {Id}", id);
                throw;
            }
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            try
            {
                var booking = await _bookingRepository.GetById(id);
                if (booking == null) return false;
                
                // Usually Cancellation logic goes here, setting status to 3 or 4 (Cancelled)
                booking.IdStatus = 4; // Assuming 4 is Cancelled/Inactiva
                booking.DeletedAt = DateTime.UtcNow;
                
                return await _bookingRepository.SoftDelete(booking);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting booking {Id}", id);
                throw;
            }
        }

        public async Task<bool> ChangeStatusAsync(Guid id, int statusId)
        {
            try
            {
                var booking = await _bookingRepository.GetById(id);
                if (booking == null) return false;

                booking.IdStatus = statusId;
                return await _bookingRepository.Update(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing status for booking {Id}", id);
                throw;
            }
        }

        public async Task<bool> Update(Guid id, BookingDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
