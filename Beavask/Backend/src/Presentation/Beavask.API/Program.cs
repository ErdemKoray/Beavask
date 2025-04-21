using Beavask.Application.Interfaces;
using Beavask.Application.Mapping;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<BeavaskDbContext>(options =>
    options.UseNpgsql(conn));

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRepositories();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
app.Run();
