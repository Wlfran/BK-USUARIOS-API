using Users_Module.Models;
using Users_Module.Models.Response;

namespace Users_Module.Services.Interface
{
    public interface IUsuariosModuloService
    {
        Task<UsuariosPendientesResponse> ObtenerPendientesAsync(
            int? numeroSolicitud,
            string? filter,
            string? empresa,
            string? contrato,
            string? areaEjecucion,
            int? anio,
            string? mes,
            string? estado,
            DateTime? fechaCreacion,
            string sortBy,
            string sortDirection,
            int skip,
            int take
        );

        Task<IEnumerable<UsuariosDetalleDTO>> GetSolicitudesByIdAsync(int idSolicitudes);
    }
}
