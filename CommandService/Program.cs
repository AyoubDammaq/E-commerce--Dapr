using CommandService.Data;
using Microsoft.EntityFrameworkCore;
using CommandService.Services;
using CommandService.Repositories;
using Prometheus;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le DbContext pour CommandDbContext
builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommandConnection")));

// Ajouter les services Dapr
builder.Services.AddDaprClient(); 

// Ajouter les services spécifiques de l'application
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();

// Ajouter les contrôleurs
builder.Services.AddControllers();

// Ajouter la configuration Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ajouter les middlewares Prometheus avant tout autre middleware
app.UseMetricServer();  // Expose les métriques via l'endpoint /metrics
app.UseHttpMetrics();   // Collecte des métriques HTTP (requêtes, latence, etc.)

// Middleware pour surveiller l'utilisation du CPU et de la mémoire
var cpuUsage = new Gauge("cpu_usage_percent", "CPU usage in percent");
var memoryUsage = new Gauge("memory_usage_bytes", "Memory usage in bytes");

app.Use(async (context, next) =>
{
    // Utilisation d'un compteur de performance pour le CPU
    var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    var ramCounter = new PerformanceCounter("Memory", "Available MBytes");

    // Met à jour les métriques
    cpuUsage.Set(cpuCounter.NextValue());
    memoryUsage.Set(ramCounter.NextValue() * 1024 * 1024);  // Convertir en octets

    await next.Invoke();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();

// Mappez les contrôleurs
app.MapControllers();

// Activer Dapr pour les événements Pub/Sub
app.UseCloudEvents();

// Exécution de l'application
app.Run();
