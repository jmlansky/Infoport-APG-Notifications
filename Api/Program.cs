using Api.Integrations.Interfaces;
using Api.Integrations;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();

builder.Services.AddScoped<INotificationStrategy, EmailStrategy>();
builder.Services.AddScoped<INotificationStrategy, PrinterStrategy>();
builder.Services.AddScoped<INotificationStrategy, PushStrategy>();


builder.Services.AddSingleton(provider =>
{
    var factory = new ConnectionFactory()
    {
        HostName = "localhost",
        Port = 5675,
        UserName = "guest",
        Password = "guest"
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton(provider =>
{
    var connection = provider.GetRequiredService<IConnection>();
    var model = connection.CreateModel();
    return model;
});

builder.Services.AddSingleton<IServiceBusSetup, RabbitMqSetupService>();
builder.Services.AddScoped<IMessageConsumer, RabbitMqMessageConsumer>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DBConnectionString");
builder.Services.AddDbContext<ApgNotificationsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<INotificationsRepository>(provider =>
{
    var dbContext = provider.GetRequiredService<ApgNotificationsDbContext>();
    return new NotificationsRepository(dbContext);
});



var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApgNotificationsDbContext>();
    context.Database.EnsureCreated();
}



// Configurar RabbitMQ
var rabbitMQSetupService = app.Services.GetRequiredService<IServiceBusSetup>();
rabbitMQSetupService.Setup();

// Crear un scope para IMessageConsumer
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var channel = services.GetRequiredService<IModel>();
    var messageConsumer = new RabbitMqMessageConsumer(app.Services, channel);
    messageConsumer.StartConsuming("eventsQueue");
}

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
