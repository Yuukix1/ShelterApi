using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ShelterApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AnimalshelterContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("Mysql")!)
);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();  
}

app.MapControllers();

app.Run();
