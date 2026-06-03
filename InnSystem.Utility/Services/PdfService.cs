using InnSystem.Model;
using InnSystem.Utility.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace InnSystem.Utility.Services
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateInvoicePdf(Booking booking, Payment payment, string customerName, string customerEmail)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(x => ComposeContent(x, booking, payment, customerName, customerEmail));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Hotel Continental").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text("Su comodidad es nuestra prioridad");
                });

                row.ConstantItem(100).AlignRight().Text($"Factura #{(new Random().Next(1000, 9999))}").FontSize(16).SemiBold();
            });
        }

        private void ComposeContent(IContainer container, Booking booking, Payment payment, string customerName, string customerEmail)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Spacing(20);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Component(new AddressComponent("Facturado a:", customerName, customerEmail));
                    row.ConstantItem(50);
                    row.RelativeItem().Component(new AddressComponent("Detalles de Reserva:", 
                        $"Habitación: {booking.IdRoomNavigation?.RoomNumber ?? booking.IdRoom.ToString()}", 
                        $"Check-In: {booking.CheckIn:dd/MM/yyyy}\nCheck-Out: {booking.CheckOut:dd/MM/yyyy}"));
                });

                column.Item().Element(x => ComposeTable(x, booking, payment));

                var totalPrice = payment.Amount;
                column.Item().AlignRight().Text($"Total Pagado: ${totalPrice:F2}").FontSize(14).SemiBold();
            });
        }

        private void ComposeTable(IContainer container, Booking booking, Payment payment)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Descripción");
                    header.Cell().Element(CellStyle).AlignRight().Text("Huéspedes");
                    header.Cell().Element(CellStyle).AlignRight().Text("Ref. Transacción");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                table.Cell().Element(CellStyle).Text("1");
                table.Cell().Element(CellStyle).Text($"Estadía ({booking.CheckIn:dd/MM} al {booking.CheckOut:dd/MM})");
                table.Cell().Element(CellStyle).AlignRight().Text($"{booking.GuestsCount}");
                table.Cell().Element(CellStyle).AlignRight().Text($"{payment.ExternalRef}");
                table.Cell().Element(CellStyle).AlignRight().Text($"${payment.Amount:F2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Gracias por elegir InnSystem Hotel. ");
                x.Span("Generado automáticamente.").FontSize(10).FontColor(Colors.Grey.Medium);
            });
        }
    }

    public class AddressComponent : IComponent
    {
        private string Title { get; }
        private string Line1 { get; }
        private string Line2 { get; }

        public AddressComponent(string title, string line1, string line2)
        {
            Title = title;
            Line1 = line1;
            Line2 = line2;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);
                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();
                column.Item().Text(Line1);
                column.Item().Text(Line2);
            });
        }
    }
}
