using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore;
using ReservationAPI.SetUp;
using System.Net;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);
IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
                      name: "AllowOrigin",
                      builder =>
                      {
                          builder
                           .WithOrigins("http://localhost:3000")
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                      });
});

var startup = new Startup(configuration);
startup.ConfigureServices(builder.Services);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthorization();

app.MapControllers();



IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .CaptureStartupErrors(false)
        .ConfigureKestrel(options =>
        {
            var ports = GetDefinedPorts(configuration);
            options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });

            options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        })
        .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
        .UseStartup<Startup>()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Build();





(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
{
    var grpcPort = config.GetValue("GRPC_PORT", 7001);
    var port = config.GetValue("PORT", 80);
    return (port, grpcPort);
}
app.Run();
public partial class Program
{

    public List<string> Args = new List<string>();

    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
    
}