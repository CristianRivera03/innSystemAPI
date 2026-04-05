using System;
using System.Threading.Tasks;
using InnSystem.Model;

namespace InnSystem.DAL.Repositories.Contract
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        // Funciones específicas mapeadas a Stored Procedures / DB Functions
        Task<Guid> RegisterPaymentAsync(Guid bookingId, int methodId, int typeId, int statusId, decimal amount, string reference);
    }
}
