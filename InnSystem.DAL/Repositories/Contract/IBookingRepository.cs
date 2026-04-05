using System;
using System.Threading.Tasks;
using InnSystem.Model;

namespace InnSystem.DAL.Repositories.Contract
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        // Funciones específicas mapeadas a Stored Procedures / DB Functions
        Task<Guid> CreateBookingAsync(Guid userId, int roomId, DateTime checkIn, DateTime checkOut, int guestsCount);
        Task<decimal> CalculateTotalPriceAsync(int roomId, DateTime checkIn, DateTime checkOut);
    }
}
