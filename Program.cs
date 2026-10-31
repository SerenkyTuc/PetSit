using Microsoft.EntityFrameworkCore;
using PetSit.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// логирование Serilog: консоль и файлы в папке logs
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/petsit-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger

// внедрение зависимостей: контекст БД регистрируется в контейнере,
// строка подключения берётся из конфигурации
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ping", () => "pong");

app.Run();
