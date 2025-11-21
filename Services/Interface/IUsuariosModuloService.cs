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
            int? mes,
            string? estado,
            DateTime? fechaCreacion,
            string sortBy,
            string sortDirection,
            int skip,
            int take
        );
    }
}
