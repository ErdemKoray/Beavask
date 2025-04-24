using Beavask.API.Service;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Beavask.Application.Mapping;
using Beavask.Infrastructure.Extensions;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Connection string -> User-Secrets 
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

//JWT -> User-Secrets 
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

// DbContext
builder.Services.AddDbContext<BeavaskDbContext>(options =>
    options.UseNpgsql(conn));

// DbContext çözümleme (Scrutor uyumlu)
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<BeavaskDbContext>());

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddApplicationDependencies();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();


// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin() // dev için, prod'da sınırlanmalı
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Controller ve Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Controllers route mapping
app.MapControllers();

app.Run();
