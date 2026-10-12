using TouristRoutes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();