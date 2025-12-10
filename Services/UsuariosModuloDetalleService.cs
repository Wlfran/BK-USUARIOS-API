using Dapper;
using Microsoft.Data.SqlClient;
using Users_Module.Models;
using Users_Module.Services.Interface;

namespace Users_Module.Services
{
    public class UsuariosModuloDetalleService : IUsuariosModuloDetalleService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public UsuariosModuloDetalleService(IConfiguration configuration, IConfiguration config)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _config = config;
        }
        public async Task GuardarBorradorAsync(BorradorEjecucionDto dto)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = @"
                            INSERT INTO Usuarios_Modulo_Borradores (IdSolicitud, Usuario, JsonContenido)
                            VALUES (@IdSolicitud, @Usuario, @JsonContenido);
                        ";

            await conn.ExecuteAsync(query, dto);
        }

        public async Task<string?> ObtenerBorradorAsync(int idSolicitud, string usuario)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var query = @"
                            SELECT TOP 1 JsonContenido
                            FROM Usuarios_Modulo_Borradores
                            WHERE IdSolicitud = @IdSolicitud AND Usuario = @Usuario
                            ORDER BY FechaGuardado DESC;
                        ";

            return await conn.QueryFirstOrDefaultAsync<string>(query, new
            {
                IdSolicitud = idSolicitud,
                Usuario = usuario
            });
        }

    }
}
