using Data.Context;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using Swashbuckle.AspNetCore.SwaggerGen;

// Cria um builder de aplica��o web com os argumentos passados
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

// Adiciona os servi�os de controllers ao builder
builder.Services.AddControllers(options =>
{
    // Insere um formato de entrada personalizado para o JsonPatch
    options.InputFormatters.Insert(0, JsonPatchSample.MyJPIF.GetJsonPatchInputFormatter());
});

// Adiciona o suporte ao NewtonsoftJson aos controllers
builder.Services.AddControllers().AddNewtonsoftJson();

// Configura as op��es de rota para usar URLs e query strings em min�sculo
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

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
            License = new OpenApiLicense() { Name = "MIT License", Url = new Uri("https://opensource.org/licenses/MIT") },
            Version = "1.0.11"
        });

        // Habilita o uso para registrar o SchemaFilter
        c.SchemaFilter<ClienteSchemaFilter>();
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
