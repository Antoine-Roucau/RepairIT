using Microsoft.EntityFrameworkCore;
using RepairIT.UserService.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("UserDb")
    ?? throw new InvalidOperationException("ConnectionStrings:UserDb manquant dans appsettings.json");
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlite(connectionString));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    await UserDbInitializer.InitialiserAsync(context, app.Logger);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "RepairIT.UserService is running");

app.Run();
