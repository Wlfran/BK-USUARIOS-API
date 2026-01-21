using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Users_Module.Models;
using Users_Module.Models.Response;
using Users_Module.Services.Interface;

namespace Users_Module.Services
{
    public class UsuariosModuloService : IUsuariosModuloService
    {
        private readonly string _connectionString;

        public UsuariosModuloService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<UsuariosPendientesResponse>
            ObtenerPendientesAsync(
                int? numeroSolicitud,
                string? filter,
                string? empresa,
                string? contrato,
                string? areaEjecucion,
                int? anio,
                string? mes,
                string? estado,
                DateTime? fechaCreacion,
                string sortBy,
                string sortDirection,
                int skip,
                int take,
                string? cedula = null)
        {
            using var conn = new SqlConnection(_connectionString);

            using var multi = await conn.QueryMultipleAsync(
                "NewWebContratistas_RWUsuarios_ObtenerPendientes",
                new
                {
                    NumeroSolicitud = numeroSolicitud,
                    Filter = filter,
                    Empresa = empresa,
                    Contrato = contrato,
                    AreaEjecucion = areaEjecucion,
                    Anio = anio,
                    Mes = mes,
                    Estado = estado,
                    FechaCreacion = fechaCreacion,

                    SortBy = sortBy,
                    SortDirection = sortDirection,
                    Skip = skip,
                    Take = take,
                    Cedula = cedula
                },
                commandType: CommandType.StoredProcedure
            );

            var data = (await multi.ReadAsync<UsuariosPendientesDto>()).ToList();

            int total = await multi.ReadFirstOrDefaultAsync<int>();

            if (total == 0 && data.Count > 0)
                total = data.Count;

            return new UsuariosPendientesResponse
            {
                Data = data,
                TotalRows = total
            };
        }

        public async Task<IEnumerable<UsuariosDetalleDTO>> GetSolicitudesByIdAsync(int idSolicitudes)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new
            {
                numSolicitud = idSolicitudes,
            };

            return await connection.QueryAsync<UsuariosDetalleDTO>(
                "NewWebContratistas_RWUsuarios_ObtenerDetallePorId",
                parameters,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<bool> ActualizarEstadoSolicitudAsync(ActualizarEstadoSolicitudDto dto)
        {
            using var conn = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@IdSolicitud", dto.IdSolicitud);
            p.Add("@Estado", dto.NuevoEstado);
            p.Add("@EsObligatorio", dto.EsObligatorio ? 1 : 0);
            p.Add("returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await conn.ExecuteAsync(
                "NewWebContratistas_RWUsuarios_ActualizarEstadoSolicitud",
                p,
                commandType: CommandType.StoredProcedure
            );

            int filas = p.Get<int>("returnValue");

            return filas > 0;
        }

        public async Task<bool> CerrarSolicitudSinNovedadesAsync(
            int idSolicitud,
            string usuario
        )
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                var p = new DynamicParameters();
                p.Add("@IdSolicitud", idSolicitud);
                p.Add("@Estado", "Cerrado");
                p.Add("@EsObligatorio", 1);
                p.Add("returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(
                    "NewWebContratistas_RWUsuarios_ActualizarEstadoSolicitud",
                    p,
                    transaction,
                    commandType: CommandType.StoredProcedure
                );

                var filas = p.Get<int>("returnValue");
                if (filas <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                const string insertHistorialSql = @"
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
                    IdSolicitud = idSolicitud,
                    Accion = "Reporte sin novedades",
                    Estado = "Cerrado",
                    Comentarios = "Solicitud reportada sin novedades",
                    EmailUsuario = usuario
                }, transaction);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }




    }
}
