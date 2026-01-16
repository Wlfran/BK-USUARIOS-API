using ClosedXML.Excel;
using System.Drawing;
using Users_Module.Services.Interface;

namespace Users_Module.Services
{
    public class UsuariosTarjetaTemplateService : IUsuariosTarjetaTemplateService
    {
        public byte[] DescargarPlantillaUsuariosTarjeta()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("UsuariosTarjeta");

            worksheet.Cell(1, 1).Value = "Nombre de pila";
            worksheet.Cell(1, 2).Value = "Apellido";
            worksheet.Cell(1, 3).Value = "Cédula (Tarjetahabiente)";
            worksheet.Cell(1, 4).Value = "Grupos de tarjetahabientes";
            worksheet.Cell(1, 5).Value = "Fecha de activación";

            var header = worksheet.Range("A1:E1");
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            worksheet.Column(1).Width = 25;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 30;
            worksheet.Column(4).Width = 30;
            worksheet.Column(5).Width = 22;

            worksheet.Column(5).Style.DateFormat.Format = "MM/dd/yyyy hh:mm:ss AM/PM";

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
