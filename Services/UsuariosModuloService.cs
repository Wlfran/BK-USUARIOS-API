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
                int? mes,
                string? estado,
                DateTime? fechaCreacion,
                string sortBy,
                string sortDirection,
                int skip,
                int take)
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
                    Take = take
                },
                commandType: CommandType.StoredProcedure
            );

            var data = (await multi.ReadAsync<UsuariosPendientesDto>()).ToList();

            int total = data.Count;

            return new UsuariosPendientesResponse
            {
                Data = data,
                TotalRows = total
            };
        }
    }
}
