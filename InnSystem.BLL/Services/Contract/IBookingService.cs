using InnSystem.DTO.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IBookingService
    {
        Task<List<BookingDTO>> GetAllAsync();
        Task<BookingDTO?> GetByIdAsync(Guid id);
        Task<BookingDTO> Create(BookingCreateDTO model);
        Task<bool> Update(Guid id, BookingDTO model);
        Task<bool> SoftDelete(Guid id);
        Task<bool> HardDelete(Guid id);
        Task<bool> ChangeStatusAsync(Guid id, int statusId);

    }
}
