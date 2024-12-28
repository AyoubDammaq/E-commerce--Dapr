using CommandService.Data;
using Microsoft.EntityFrameworkCore;
using CommandService.Services;
using CommandService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();

// Ajouter le DbContext pour CommandDbContext
builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommandConnection")));

// Ajouter les services Dapr
builder.Services.AddDaprClient();




// Ajouter les contr�leurs
builder.Services.AddControllers();

// Ajouter la configuration Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configurer le pipeline de requ�tes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mappez les contr�leurs
app.MapControllers();

// Activer Dapr pour les �v�nements Pub/Sub
app.UseCloudEvents();

// Ex�cution de l'application
app.Run();
