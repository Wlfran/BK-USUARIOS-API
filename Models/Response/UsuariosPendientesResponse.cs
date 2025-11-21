namespace Users_Module.Models.Response
{
    public class UsuariosPendientesResponse
    {
        public List<UsuariosPendientesDto> Data { get; set; } = new List<UsuariosPendientesDto>();
        public int TotalRows { get; set; }
    }
}
