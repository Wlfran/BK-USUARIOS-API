using Microsoft.AspNetCore.Mvc;
using Users_Module.Models;
using Users_Module.Services;

namespace Users_Module.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalContratoController : ControllerBase
    {
        private readonly IPersonalContratoService _service;

        public PersonalContratoController(IPersonalContratoService service)
        {
            _service = service;
        }

        [HttpGet("{numeroContrato}")]
        public async Task<IActionResult> GetPersonalPorContrato(string numeroContrato)
        {
            var data = await _service.ObtenerPersonalPorContrato(numeroContrato);

            if (data == null || !data.Any())
                return Ok(new { mensaje = "No existe personal asociado a este contrato." });

            return Ok(data);
        }

        [HttpPost("preview-excel")]
        public async Task<IActionResult> PreviewExcel(IFormFile archivo)
        {
            var resultado = await _service.PreviewExcelAsync(archivo);
            return Ok(resultado);
        }

        [HttpPost("confirmar-carga")]
        public async Task<IActionResult> ConfirmarCarga(
            [FromBody] IEnumerable<UsuarioTarjetaPreviewDto> usuarios,
            [FromQuery] string? usuario)
        {
            var insertados = await _service.ConfirmarCargaAsync(usuarios, usuario);

            return Ok(new
            {
                mensaje = "Carga realizada correctamente",
                registrosInsertados = insertados
            });
        }

        [HttpGet("usuarios-tarjeta")]
        public async Task<IActionResult> ObtenerUsuariosTarjeta(
            [FromQuery] string? nombrePila,
            [FromQuery] string? apellido,
            [FromQuery] string? cedula,
            [FromQuery] string? grupo,
            [FromQuery] string? usuarioCarga,
            [FromQuery] int? anio,
            [FromQuery] string? mes,
            [FromQuery] DateTime? fechaActivacion,
            [FromQuery] DateTime? fechaCarga,
            [FromQuery] string? filter,
            [FromQuery] string sortBy = "FechaCarga",
            [FromQuery] string sortDirection = "DESC",
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10
        )
        {
            var (data, totalRows) = await _service.ObtenerUsuariosTarjetaAsync(
                nombrePila,
                apellido,
                cedula,
                grupo,
                usuarioCarga,
                anio,
                mes,
                fechaActivacion,
                fechaCarga,
                filter,
                sortBy,
                sortDirection,
                skip,
                take
            );

            return Ok(new
            {
                data,
                totalRows,
                skip,
                take
            });
        }

    }
}
