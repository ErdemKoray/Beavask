using Beavask.Application.Interface;
using Beavask.Application.Mapping;
using Beavask.Infrastructure.Extensions;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

// DbContext
builder.Services.AddDbContext<BeavaskDbContext>(options =>
    options.UseNpgsql(conn));

// DbContext çözümleme (Scrutor uyumlu)
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<BeavaskDbContext>());

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddApplicationDependencies();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

// Controller ve Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
