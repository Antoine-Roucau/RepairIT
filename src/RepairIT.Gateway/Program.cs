var builder = WebApplication.CreateBuilder(args);

// Routes et destinations lues dans la section "ReverseProxy" de appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Origines autorisées (client Blazor) lues dans la configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowBlazorClient");

app.MapReverseProxy();

// Vérification que la Gateway tourne : http://localhost:5010/
app.MapGet("/", () => "RepairIT.Gateway is running");

app.Run();