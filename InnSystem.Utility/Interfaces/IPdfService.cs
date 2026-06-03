using InnSystem.Model;

namespace InnSystem.Utility.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateInvoicePdf(Booking booking, Payment payment, string customerName, string customerEmail);
    }
}
