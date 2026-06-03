using System;

namespace InnSystem.DTO.Wompi
{
    public class WompiWebhookDTO
    {
        public string IdTransaccion { get; set; } = string.Empty;
        public string Monto { get; set; } = string.Empty;
        public string ResultadoTransaccion { get; set; } = string.Empty;
        public string IdExterno { get; set; } = string.Empty;
        public WompiEnlacePagoDTO EnlacePago { get; set; } = new WompiEnlacePagoDTO();
    }

    public class WompiEnlacePagoDTO
    {
        public string IdentificadorEnlaceComercio { get; set; } = string.Empty;
    }
}
