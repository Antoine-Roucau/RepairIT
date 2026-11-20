using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RepairIT.UserService.DB;
using RepairIT.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("UserDb")
    ?? throw new InvalidOperationException("ConnectionStrings:UserDb manquant dans appsettings.json");

builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();

// echange des enums en string dans les json
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cree et init BD si besoin
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await UserDbInitializer.InitialiserAsync(context, app.Logger);
}

app.UseAuthorization();

app.MapControllers();

// Verif
app.MapGet("/", () => "RepairIT.UserService is running");

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();