using Users_Module.Models;

namespace Users_Module.Services.Interface
{
    public interface IUsuariosModuloDetalleService
    {
        Task GuardarBorradorAsync(BorradorEjecucionDto dto);
        Task<string?> ObtenerBorradorAsync(int idSolicitud, string usuario);
    }
}
