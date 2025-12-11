using Microsoft.AspNetCore.Mvc;
using Users_Module.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users_Module.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosModuloController : ControllerBase
    {
        private readonly IUsuariosModuloService _usuariosModuloService;

        public UsuariosModuloController(IUsuariosModuloService usuariosModuloService)
        {
            _usuariosModuloService = usuariosModuloService;
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPendientes(
             int? numeroSolicitud,
             string? filter,
             string? empresa,
             string? contrato,
             string? areaEjecucion,
             int? anio,
             string? mes,
             string? estado,
             DateTime? fechaCreacion,
             string sortBy = "IdSolicitud",
             string sortDirection = "ASC",
             int skip = 0,
             int take = 10, string? cedula = null)
        {
            var result = await _usuariosModuloService.ObtenerPendientesAsync(
                numeroSolicitud, filter, empresa, contrato, areaEjecucion,
                anio, mes, estado, fechaCreacion,
                sortBy, sortDirection, skip, take, cedula);

            return Ok(result);
        }

        [HttpGet("pendientesById")]
        public async Task<IActionResult> GetSolicitudesById(int numSolicitud)
        {
            var data = await _usuariosModuloService.GetSolicitudesByIdAsync(numSolicitud);

            return Ok(new { data, totalRows = data.Count() });
        }




    }
}

