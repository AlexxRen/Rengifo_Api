using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Interfaces;
using Rengifo_Api.Repositories;
using System.Reflection;
using LaunchDarkly.Sdk;


var builder = WebApplication.CreateBuilder(args);

//  Configuración de LaunchDarkly
builder.Services.AddSingleton<ILdClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var ldSection = configuration.GetSection("LaunchDarkly");

    var sdkKey = ldSection.GetValue<string>("SdkKey") ?? "sdk-local-relay";
    var useRelay = ldSection.GetValue<bool>("UseRelay");
    var relayBaseUri = ldSection.GetValue<string>("RelayBaseUri");

    var configBuilder = Configuration.Builder(sdkKey);

    if (useRelay && !string.IsNullOrEmpty(relayBaseUri))
    {
        // Apuntar al Relay Proxy en lugar del SaaS directo
        configBuilder
            .ServiceEndpoints(Components.ServiceEndpoints()
                .RelayProxy(relayBaseUri));
    }

    return new LdClient(configBuilder.Build());
});
// Fin Configuración de LaunchDarkly

// MVC
builder.Services.AddControllersWithViews();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Incluir documentación XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

// REGISTRAR TU REPOSITORIO EN MEMORIA
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();

var app = builder.Build();

// Habilitar Swagger siempre (o solo en dev si quieres)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API v1");
    c.RoutePrefix = string.Empty; /*Swagger en http://localhost:5000/*/
});

// Asegurarse que LD se conecte al inicio
var ldClient = app.Services.GetRequiredService<ILdClient>();
if (!ldClient.Initialized)
{
    Console.WriteLine("LaunchDarkly client no se pudo inicializar todavía...");
}
// Fin check de inicialización de LaunchDarkly

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();