using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Load secrets
builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/gamestore",

            ValidateAudience = true,
            ValidAudience = "gamestore-api",

            //ValidateSignatureLast = false,

            ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                "http://localhost:8080/realms/gamestore/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever() { RequireHttps = false }
            )
        };

        //opt.Authority = "http://localhost:8080/realms/gamestore";
        //opt.Audience = "gamestore-api";
        //// If we want to use HTTP schema instead of HTTPS in all Keycloak URLs
        //opt.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorizationBuilder();

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