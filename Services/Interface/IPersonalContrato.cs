using Users_Module.Models;
using Users_Module.Models.Response;

namespace Users_Module.Services
{
    public interface IPersonalContratoService
    {
        Task<IEnumerable<PersonalContratoDto>> ObtenerPersonalPorContrato(string numeroContrato);
        Task<CargaExcelPreviewResponse> PreviewExcelAsync(IFormFile archivo);

        Task<int> ConfirmarCargaAsync(
            IEnumerable<UsuarioTarjetaPreviewDto> usuariosValidos,
            string? usuarioCarga
        );

        Task<(IEnumerable<UsuarioTarjetaDto> Data, int TotalRows)>
            ObtenerUsuariosTarjetaAsync(
                string? nombrePila,
                string? apellido,
                string? cedula,
                string? grupo,
                string? usuarioCarga,
                int? anio,
                string? mes,
                DateTime? fechaActivacion,
                DateTime? fechaCarga,
                string? filter,
                string sortBy,
                string sortDirection,
                int skip,
                int take
            );
    }
}
