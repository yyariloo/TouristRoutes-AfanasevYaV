using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TouristRoutes.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton(new TouristRoutes.Services.RouteService());
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();