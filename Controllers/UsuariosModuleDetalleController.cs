using Microsoft.AspNetCore.Mvc;
using Users_Module.Models;
using Users_Module.Models.Request;
using Users_Module.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users_Module.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosModuleDetalleController : ControllerBase
    {
        private readonly IUsuariosModuloDetalleService _usuariosModuloService;

        public UsuariosModuleDetalleController(IUsuariosModuloDetalleService usuariosModuloService)
        {
            _usuariosModuloService = usuariosModuloService;
        }

        [HttpPost("guardar-borrador")]
        public async Task<IActionResult> GuardarBorrador([FromBody] BorradorEjecucionDto dto)
        {
            try
            {
                await _usuariosModuloService.GuardarBorradorAsync(dto);
                return Ok(new { ok = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, error = ex.Message, stack = ex.StackTrace });
            }
        }


        [HttpGet("obtener-borrador/{idSolicitud:int}/{usuario}")]
        public async Task<IActionResult> ObtenerBorrador(int idSolicitud, string usuario)
        {
            var json = await _usuariosModuloService.ObtenerBorradorAsync(idSolicitud, usuario);
            return Ok(new { json });
        }

        [HttpPost("registrar-retiro")]
        public async Task<IActionResult> RegistrarRetiro([FromBody] RegistrarRetiroRequest request)
        {
            if (request.Contratistas == null || request.Contratistas.Count == 0)
                return BadRequest("No hay contratistas para registrar.");

            await _usuariosModuloService.RegistrarRetirosAsync(request);

            return Ok(new { mensaje = "Retiros registrados correctamente." });
        }
    }
}
