using Users_Module.Models;
using Users_Module.Models.Request;

namespace Users_Module.Services.Interface
{
    public interface IUsuariosModuloDetalleService
    {
        Task GuardarBorradorAsync(BorradorEjecucionDto dto);
        Task<string?> ObtenerBorradorAsync(int idSolicitud, string usuario);
        Task RegistrarRetirosAsync(RegistrarRetiroRequest request);
        Task<IEnumerable<SolicitudDetalleDto>> ObtenerDetalleAsync(int idSolicitud);
    }
}
