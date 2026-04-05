using System;
using System.Linq;
using System.Threading.Tasks;
using InnSystem.DAL.DBConext;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace InnSystem.DAL.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly InnDbContext _dbContext;

        public PaymentRepository(InnDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> RegisterPaymentAsync(Guid bookingId, int methodId, int typeId, int statusId, decimal amount, string reference)
        {
            var query = "SELECT fn_register_payment({0}, {1}, {2}, {3}, {4}, {5}) AS Value";
            
            var result = await _dbContext.Database
                .SqlQueryRaw<Guid>(query, bookingId, methodId, typeId, statusId, amount, reference)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
