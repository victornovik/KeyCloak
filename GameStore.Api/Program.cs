using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Load secrets
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

var port = Environment.GetEnvironmentVariable("PORT") ?? "unknown";
Console.WriteLine($"✅ ASP.NET Core is listening on port: {port}");

app.Run();