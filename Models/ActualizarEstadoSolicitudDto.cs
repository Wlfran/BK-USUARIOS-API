namespace Users_Module.Models
{
    public class ActualizarEstadoSolicitudDto
    {
        public int IdSolicitud { get; set; }
        public string NuevoEstado { get; set; } = string.Empty;
        public bool EsObligatorio { get; set; }
    }
}
