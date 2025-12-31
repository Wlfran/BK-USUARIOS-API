namespace Users_Module.Models.Response
{
    public class CargaExcelPreviewResponse
    {
        public int TotalFilas { get; set; }
        public int FilasValidas { get; set; }
        public int FilasConError { get; set; }

        public List<UsuarioTarjetaPreviewDto> Registros { get; set; } = [];
    }
}
