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

builder.Services.AddMetrics(); // Ajouter les métriques
builder.Services.AddHealthChecks(); // Facultatif

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapMetrics();


app.UseAuthorization();

// Mappez les contrôleurs
app.MapControllers();

// Activer Dapr pour les événements Pub/Sub
app.UseCloudEvents();

// Exécution de l'application
app.Run();
