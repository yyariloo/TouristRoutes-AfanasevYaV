using System.Text.Json.Serialization;
using TouristRoutes;
using TouristRoutes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddSingleton<IRoutePricingService, RoutePricingService>();
builder.Services.AddSingleton<IGpxParser, GpxParser>();
builder.Services.AddHttpClient<IMapService, MapService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.MapControllers();

app.Run();