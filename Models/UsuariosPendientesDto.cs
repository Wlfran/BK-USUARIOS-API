namespace Users_Module.Models
{
    public class UsuariosPendientesDto
    {
        public int NumeroSolicitud { get; set; }
        public string Formulario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NumeroContrato { get; set; }
        public string Empresa { get; set; }
        public string AreaEjecucion { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Estado { get; set; }
    }
}
