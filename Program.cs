using System.Text.Json.Serialization;
using TouristRoutes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.MapControllers();

app.Run();