using Microsoft.AspNetCore.Mvc;
using Users_Module.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                return NotFound(new { mensaje = "No existe personal asociado a este contrato." });

            return Ok(data);
        }
    }
}
