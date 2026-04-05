using InnSystem.DTO.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IBookingService
    {
        Task<List<BookingDTO>> GetAllAsync();
        Task<BookingCreateDTO> Create(BookingCreateDTO model);
        Task<bool> Update(BookingDTO model);
        Task<bool> SoftDelete(Guid id);
        Task<bool> HardDelete(Guid id);

    }
}
