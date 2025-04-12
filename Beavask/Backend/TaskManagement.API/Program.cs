using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Configuration.AddUserSecrets<Program>();
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseNpgsql(conn));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
