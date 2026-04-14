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

        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IGenericRepository<Booking> bookingRepository, IMapper mapper, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookingDTO>> GetAllAsync()
        {
            try
            {
                var queryBooking = _bookingRepository.Query();
                var listBooking = await queryBooking.Include(b => b.IdUserNavigation).ToListAsync();

                return _mapper.Map<List<BookingDTO>>(listBooking);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting the bookings");
                throw;
            }
        }

        public Task<BookingCreateDTO> Create(BookingCreateDTO model)
        {
            throw new NotImplementedException();
        }


        public Task<bool> HardDelete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SoftDelete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BookingDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
