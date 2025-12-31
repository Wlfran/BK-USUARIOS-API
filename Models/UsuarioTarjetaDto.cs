namespace Users_Module.Models
{
    public class UsuarioTarjetaDto
    {
        public int IdTarjetahabiente { get; set; }
        public string NombrePila { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string GrupoTarjetahabiente { get; set; } = null!;

        public DateTime FechaActivacion { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; } = null!;

        public DateTime FechaCarga { get; set; }
        public string? UsuarioCarga { get; set; }
    }
}
