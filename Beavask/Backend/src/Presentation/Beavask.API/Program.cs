using Beavask.API.Service;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Beavask.Application.Mapping;
using Beavask.Infrastructure.Extensions;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Connection string -> User-Secrets 
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

//JWT -> User-Secrets 
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

//GitHub Project Info
var ClientId = builder.Configuration["GitHub:ClientId"];
var ClientSecret = builder.Configuration["GitHub:ClientSecret"];

// DbContext
builder.Services.AddDbContext<BeavaskDbContext>(options =>
    options.UseNpgsql(conn));

// DbContext çözümleme (Scrutor uyumlu)
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<BeavaskDbContext>());

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddApplicationDependencies();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentCompanyService, CurrentCompanyService>();
builder.Services.AddScoped<IRepoService, RepoService>();
builder.Services.AddScoped<IMailService, GmailMailService>();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

// JWT Auth
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

// Swagger + JWT support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Beavask API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Enter 'Bearer {token}'",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors();

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

// 🔐 Auth Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
