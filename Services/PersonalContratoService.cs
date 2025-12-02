using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Users_Module.Models;

namespace Users_Module.Services
{
    public class PersonalContratoService : IPersonalContratoService
    {
        private readonly IConfiguration _config;

        public PersonalContratoService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<PersonalContratoDto>> ObtenerPersonalPorContrato(string numeroContrato)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connectionString))
            {
                var result = await conn.QueryAsync<PersonalContratoDto>(
                    "NewWebContratistas_RWUsuarios_ObtenerPersonaPorContrato",
                    new { NoContrato = numeroContrato },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
    }
}
