using Dapper;
using Microsoft.Data.SqlClient;
using Users_Module.Models;
using Users_Module.Models.Request;
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

        public async Task RegistrarRetirosAsync(RegistrarRetiroRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var c in request.Contratistas)
                {
                    var sql = @"
                    INSERT INTO Modulo_Usuarios_Solicitudes_Detalle
                    (IdSolicitud, Cedula, Nombre, FechaInicio, FechaRetiro, Comentarios, CreadoPor, Ticket_ESV)
                    VALUES
                    (@IdSolicitud, @Cedula, @Nombre, @FechaInicio, @FechaRetiro, @Comentarios, @CreadoPor, @TicketESV)";

                    await connection.ExecuteAsync(sql, new
                    {
                        IdSolicitud = request.IdSolicitud,
                        Cedula = c.Cedula,
                        Nombre = c.Nombre,
                        FechaInicio = c.FechaInicio,
                        FechaRetiro = c.FechaRetiro,
                        Comentarios = c.Comentarios,
                        CreadoPor = request.Usuario,
                        TicketESV = c.TicketESV
                    }, transaction);
                }

                var totalPersonas = request.Contratistas.Count;

                var insertHistorialSql = @"
                INSERT INTO Modulo_Usuarios_HistorialReportes
                (
                    IdSolicitud,
                    FechaAccion,
                    Accion,
                    Estado,
                    Comentarios,
                    EmailUsuario
                )
                VALUES
                (
                    @IdSolicitud,
                    GETDATE(),
                    @Accion,
                    @Estado,
                    @Comentarios,
                    @EmailUsuario
                )";

                await connection.ExecuteAsync(insertHistorialSql, new
                {
                    IdSolicitud = request.IdSolicitud,
                    Accion = "Reportar retiro",
                    Estado = "En proceso",
                    Comentarios = $"Se retiraron {totalPersonas} personas",
                    EmailUsuario = request.Usuario
                }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<IEnumerable<SolicitudDetalleDto>> ObtenerDetalleAsync(int idSolicitud)
        {
            using var conn = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            var query = @"
                ;WITH UltimaFecha AS (
                    SELECT 
                        MAX(CONVERT(datetime2(0), FechaRegistro)) AS FechaReciente
                    FROM Modulo_Usuarios_Solicitudes_Detalle
                    WHERE IdSolicitud = @IdSolicitud
                )
                SELECT 
                    d.IdDetalle,
                    d.IdSolicitud,
                    d.Cedula,
                    d.Nombre,
                    d.FechaInicio,
                    d.FechaRetiro,
                    d.Comentarios,
                    d.CreadoPor,
                    d.FechaRegistro,
                    d.Ticket_ESV
                FROM Modulo_Usuarios_Solicitudes_Detalle d
                INNER JOIN UltimaFecha u 
                    ON CONVERT(datetime2(0), d.FechaRegistro) = u.FechaReciente
                WHERE d.IdSolicitud = @IdSolicitud
                ORDER BY d.IdDetalle;
            ";

            return await conn.QueryAsync<SolicitudDetalleDto>(
                query,
                new { IdSolicitud = idSolicitud }
            );
        }



    }
}
