using AdminIvoire.Application;
using AdminIvoire.Infrastructure;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.WebApi.BackgroundServices;
using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services
.AddApplication()
.AddInfrastructure(builder.Configuration);

builder.Services.AddBackgroundServices();

await builder.Services.ApplyMigrationAsync<LocaliteContext>(default);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapCarter();

await app.RunAsync();
