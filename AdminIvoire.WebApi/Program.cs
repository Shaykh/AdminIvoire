using AdminIvoire.Application;
using AdminIvoire.Infrastructure;
using AdminIvoire.Infrastructure.Persistence;
using AdminIvoire.WebApi.Authentication;
using AdminIvoire.WebApi.BackgroundServices;
using Carter;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("Admin Ivoire Api");
builder.Services.AddCarter();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

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
    app.MapScalarApiReference(options =>
    {
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapCarter();

await app.RunAsync();
