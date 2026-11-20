using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.DB;
using RepairIT.CatalogService.Services;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CatalogDb")
    ?? throw new InvalidOperationException("ConnectionStrings:CatalogDb manquant dans appsettings.json");

builder.Services.AddDbContext<CatalogDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IOrdinateurService, OrdinateurService>();


// echange des enums en string dans les json
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cree et init BD si besoin
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await CatalogDbInitializer.InitialiserAsync(context, app.Logger);
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Verif
app.MapGet("/", () => "RepairIT.CatalogService is running");

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();