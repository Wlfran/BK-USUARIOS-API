namespace Users_Module.Models
{
    public class UsuarioTarjetaPreviewDto
    {
        public int FilaExcel { get; set; }

        public string NombrePila { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string GrupoTarjetahabiente { get; set; } = null!;
        public DateTime? FechaActivacion { get; set; }

        public bool TieneError { get; set; }
        public string? MensajeError { get; set; }
    }
}
