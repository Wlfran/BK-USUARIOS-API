namespace Users_Module.Models
{
    public class UsuarioContratistasSolicitudDetalleDto
    {
        public int IdSolicitud { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaRetiro { get; set; }
        public string? Comentarios { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
    }
}
