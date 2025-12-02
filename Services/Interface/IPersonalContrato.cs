using Users_Module.Models;

namespace Users_Module.Services
{
    public interface IPersonalContratoService
    {
        Task<IEnumerable<PersonalContratoDto>> ObtenerPersonalPorContrato(string numeroContrato);
    }
}
