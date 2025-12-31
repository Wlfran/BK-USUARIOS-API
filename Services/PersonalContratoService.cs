using Dapper;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using System.Data;
using Users_Module.Models;
using Users_Module.Models.Response;

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

        public async Task<CargaExcelPreviewResponse> PreviewExcelAsync(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                throw new Exception("Archivo inválido");

            var resultado = new List<UsuarioTarjetaPreviewDto>();
            var connectionString = _config.GetConnectionString("DefaultConnection");

            HashSet<string> cedulasExistentes;
            using (var conn = new SqlConnection(connectionString))
            {
                var cedulas = await conn.QueryAsync<string>(
                    "SELECT Cedula FROM Usuarios_Tarjeta"
                );

                cedulasExistentes = cedulas.ToHashSet();
            }

            System.Text.Encoding.RegisterProvider(
                System.Text.CodePagesEncodingProvider.Instance
            );

            using var stream = archivo.OpenReadStream();
            using var reader = ExcelReaderFactory.CreateReader(stream);

            int filaExcel = 0;

            while (reader.Read())
            {
                filaExcel++;

                if (filaExcel == 1)
                    continue;

                var dto = new UsuarioTarjetaPreviewDto
                {
                    FilaExcel = filaExcel,
                    NombrePila = reader.GetValue(0)?.ToString()?.Trim() ?? "",
                    Apellido = reader.GetValue(1)?.ToString()?.Trim() ?? "",
                    Cedula = reader.GetValue(2)?.ToString()?.Trim() ?? "",
                    GrupoTarjetahabiente = reader.GetValue(3)?.ToString()?.Trim() ?? ""
                };

                if (DateTime.TryParse(reader.GetValue(4)?.ToString(), out var fecha))
                    dto.FechaActivacion = fecha;
                else
                {
                    dto.TieneError = true;
                    dto.MensajeError = "Fecha de activación inválida";
                }

                if (string.IsNullOrWhiteSpace(dto.Cedula))
                {
                    dto.TieneError = true;
                    dto.MensajeError = "Cédula vacía";
                }
                else if (cedulasExistentes.Contains(dto.Cedula))
                {
                    dto.TieneError = true;
                    dto.MensajeError = "La cédula ya existe";
                }
                else if (string.IsNullOrWhiteSpace(dto.NombrePila)
                      || string.IsNullOrWhiteSpace(dto.Apellido))
                {
                    dto.TieneError = true;
                    dto.MensajeError = "Nombre o apellido vacío";
                }

                resultado.Add(dto);
            }

            return new CargaExcelPreviewResponse
            {
                TotalFilas = resultado.Count,
                FilasValidas = resultado.Count(x => !x.TieneError),
                FilasConError = resultado.Count(x => x.TieneError),
                Registros = resultado
            };
        }

        public async Task<int> ConfirmarCargaAsync(
            IEnumerable<UsuarioTarjetaPreviewDto> usuariosValidos,
            string? usuarioCarga
        )
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connectionString);

            var registros = usuariosValidos
                .Where(x => !x.TieneError)
                .Select(x => new
                {
                    x.NombrePila,
                    x.Apellido,
                    x.Cedula,
                    x.GrupoTarjetahabiente,
                    FechaActivacion = x.FechaActivacion!.Value,
                    FechaCarga = DateTime.Now,
                    UsuarioCarga = usuarioCarga
                });

            var sql = @"
                INSERT INTO Usuarios_Tarjeta
                (NombrePila, Apellido, Cedula, GrupoTarjetahabiente, FechaActivacion, FechaCarga, UsuarioCarga)
                VALUES
                (@NombrePila, @Apellido, @Cedula, @GrupoTarjetahabiente, @FechaActivacion, @FechaCarga, @UsuarioCarga)
            ";

            var insertados = await conn.ExecuteAsync(sql, registros);
            return insertados;
        }

        public async Task<(IEnumerable<UsuarioTarjetaDto> Data, int TotalRows)>
        ObtenerUsuariosTarjetaAsync(
            string? nombrePila,
            string? apellido,
            string? cedula,
            string? grupo,
            string? usuarioCarga,
            int? anio,
            string? mes,
            DateTime? fechaActivacion,
            DateTime? fechaCarga,
            string? filter,
            string sortBy,
            string sortDirection,
            int skip,
            int take
        )
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connectionString);

            var parameters = new
            {
                NombrePila = nombrePila,
                Apellido = apellido,
                Cedula = cedula,
                Grupo = grupo,
                UsuarioCarga = usuarioCarga,
                Anio = anio,
                Mes = mes,
                FechaActivacion = fechaActivacion,
                FechaCarga = fechaCarga,
                Filter = filter,
                SortBy = sortBy,
                SortDirection = sortDirection,
                Skip = skip,
                Take = take
            };

            using var multi = await conn.QueryMultipleAsync(
                "NewWebContratistas_RWUsuarios_ObtenerUsuariosTarjetas",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var data = await multi.ReadAsync<UsuarioTarjetaDto>();
            var totalRows = await multi.ReadSingleAsync<int>();

            return (data, totalRows);
        }

    }
}
