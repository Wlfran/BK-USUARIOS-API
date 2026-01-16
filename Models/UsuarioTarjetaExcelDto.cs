namespace Users_Module.Models
{
    public class UsuarioTarjetaExcelDto
    {
        public string NombrePila { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string GrupoTarjetahabiente { get; set; } = null!;
        public DateTime FechaActivacion { get; set; }
    }
}
