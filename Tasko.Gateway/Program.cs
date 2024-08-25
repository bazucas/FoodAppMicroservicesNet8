using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Tasko.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppAuthentication();

_ = builder.Configuration.AddJsonFile(builder.Environment.EnvironmentName.ToLower().Equals("production")
    ? "ocelot.Production.json" : "ocelot.json", false, true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot().GetAwaiter().GetResult();
app.Run();