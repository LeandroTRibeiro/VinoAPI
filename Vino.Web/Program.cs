using Autofac;
using Autofac.Extensions.DependencyInjection;
using BetterThanYou.Core;
using BetterThanYou.Infrastructure;
using BetterThanYou.Infrastructure.Data;
using BetterThanYou.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;

// ✅ CARREGA O .ENV
if (File.Exists(".env"))
{
    Env.Load();
    Console.WriteLine("✅ Arquivo .env carregado");
}
else
{
    Console.WriteLine("ℹ️ Arquivo .env não encontrado, usando variáveis de ambiente do sistema");
}

// 🔍 DEBUG - Mostra variáveis de ambiente
Console.WriteLine("=== DEBUG VARIÁVEIS DE AMBIENTE ===");
Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"JWT_KEY existe? {!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JWT_KEY"))}");
Console.WriteLine($"JWT_KEY length: {Environment.GetEnvironmentVariable("JWT_KEY")?.Length ?? 0}");
Console.WriteLine($"JWT_ISSUER: {Environment.GetEnvironmentVariable("JWT_ISSUER")}");
Console.WriteLine($"JWT_AUDIENCE: {Environment.GetEnvironmentVariable("JWT_AUDIENCE")}");
Console.WriteLine("===================================");

var builder = WebApplication.CreateBuilder(args);

// ✅ CONFIGURA A CONNECTION STRING
var connectionString = Environment.GetEnvironmentVariable("DB_HOST") != null
    ? $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
      $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
      $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
      $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
      $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
      "Pooling=true;"
    : builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"📊 Connection String configurada: {connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0))}...");

// ✅ CONFIGURA O JWT
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") 
    ?? builder.Configuration["Jwt:Key"];
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
    ?? builder.Configuration["Jwt:Issuer"];
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
    ?? builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key não configurada!");
}

Console.WriteLine($"🔐 JWT configurado - Issuer: {jwtIssuer}, Audience: {jwtAudience}");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<DefaultCoreModule>();
    containerBuilder.RegisterModule<InfrastructureModule>();
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ APLICA MIGRATIONS AUTOMATICAMENTE
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Aplicando migrations...");
        context.Database.Migrate();
        logger.LogInformation("✅ Migrations aplicadas com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Erro ao aplicar migrations");
        throw;
    }
}

app.Run();