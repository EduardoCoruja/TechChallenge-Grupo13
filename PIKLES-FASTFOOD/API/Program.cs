using App.Application.Interfaces;
using App.Application.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllers();
// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Configura��o do SwaggerGen para gerar a documenta��o da Web API.
builder.Services.AddSwaggerGen(
    c =>
    {
        // Habilita o uso de anota��es (como [SwaggerOperation]) para melhorar a documenta��o.
        c.EnableAnnotations();
        // Define a vers�o da documenta��o Swagger como "v1".
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Swagger Documenta��o Web API",
            Description = "Um exemplo de como fornecer a documenta��o para APIs.",
            Contact = new OpenApiContact() { Name = "Tech Challenge - Grupo 13", Email = "grupo13@fiap.com" },
            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") }
        });
    }
);

var app = builder.Build();

// Configure a pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseReDoc(c =>
    {
        c.DocumentTitle = "REDOC API Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
