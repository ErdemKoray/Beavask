using Beavask.Application.Interfaces;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<BeavaskDbContext>(options =>
    options.UseNpgsql(conn));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
