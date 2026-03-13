using BApi.Controller;
using DotNetEnv;
using BApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var url = "http://0.0.0.0:7797";
if (!string.IsNullOrEmpty(url))
{
    builder.WebHost.UseUrls(url);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowExpo", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "bapi.db");
var connectionString = $"Data Source={dbPath}";

builder.Services.AddDbContext<DbContextBApi>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BApi.Repositories.IProductRepository, BApi.Repositories.ProductRepository>();
builder.Services.AddScoped<BApi.Services.ProductService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContextBApi>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowExpo");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapProductEndpoints();

app.Run();