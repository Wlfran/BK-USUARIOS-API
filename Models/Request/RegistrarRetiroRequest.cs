namespace Users_Module.Models.Request
{
    public class RegistrarRetiroRequest
    {
        public int IdSolicitud { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public List<UsuarioContratistasSolicitudDetalleDto> Contratistas { get; set; } = new();
    }
}
