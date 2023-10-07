using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.

// Criar uma vari�vel que armazena a string de conex�o com o banco de dados MySQL.
var connectionStringMysql = builder.Configuration.GetConnectionString("ConnectionMysql");

// Adicionar um servi�o do tipo MySQLContext ao objeto builder.Services.
builder.Services.AddDbContext<MySQLContext>(option => option.UseMySql(
    connectionStringMysql, // Usar a string de conex�o.
    ServerVersion.AutoDetect(connectionStringMysql), // Especificar a vers�o do servidor MySQL.
    builder => builder.MigrationsAssembly("API") // Especifica o assembly do projeto que cont�m as classes de migra��es do EF Core.
    )
);


builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

builder.Services.AddControllers().AddNewtonsoftJson();

// Configurar os servi�os relacionados aos controladores.
builder.Services.AddControllers();

// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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
