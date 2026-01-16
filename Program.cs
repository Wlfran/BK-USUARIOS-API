using Users_Module.Services;
using Users_Module.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAndProd", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://tu-dominio-produccion.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsuariosModuloService, UsuariosModuloService>();
builder.Services.AddScoped<IPersonalContratoService, PersonalContratoService>();
builder.Services.AddScoped<IUsuariosModuloDetalleService, UsuariosModuloDetalleService>();
builder.Services.AddScoped<IUsuariosTarjetaTemplateService, UsuariosTarjetaTemplateService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalAndProd");

app.UseAuthorization();

app.MapControllers();

app.Run();
