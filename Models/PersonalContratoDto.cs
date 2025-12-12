using System.Transactions;

namespace Users_Module.Models
{
    public class PersonalContratoDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string correo { get; set; }
        public string celular { get; set; } = string.Empty;
    }
}
