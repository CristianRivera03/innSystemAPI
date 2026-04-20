using System;
using System.Linq;
using System.Threading.Tasks;
using InnSystem.DAL.DBConext;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace InnSystem.DAL.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly InnDbContext _dbContext;

        public BookingRepository(InnDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateBookingAsync(Guid userId, int roomId, DateTime checkIn, DateTime checkOut, int guestsCount)
        {
            // Agregamos las comillas dobles escapadas al alias: AS \"Value\"
            var query = "SELECT fn_create_booking({0}, {1}, {2}::date, {3}::date, {4}) AS \"Value\"";

            var result = await _dbContext.Database
                .SqlQueryRaw<Guid>(query, userId, roomId, checkIn, checkOut, guestsCount)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<decimal> CalculateTotalPriceAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var query = "SELECT fn_calculate_total_price({0}, {1}, {2}) AS Value";
            
            var result = await _dbContext.Database
                .SqlQueryRaw<decimal>(query, roomId, checkIn, checkOut)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
