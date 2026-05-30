using System;

namespace InnSystem.DTO.Wompi
{
    public class WompiWebhookDTO
    {
        public string Evento { get; set; } = string.Empty;
        public WompiDataDTO Data { get; set; } = new WompiDataDTO();
    }

    public class WompiDataDTO
    {
        public WompiTransaccionDTO Transaccion { get; set; } = new WompiTransaccionDTO();
        public WompiEnlacePagoDTO EnlacePago { get; set; } = new WompiEnlacePagoDTO();
    }

    public class WompiTransaccionDTO
    {
        public string IdTransaccion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
    }

    public class WompiEnlacePagoDTO
    {
        public string IdentificadorEnlaceComercio { get; set; } = string.Empty;
    }
}
