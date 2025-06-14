

using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XpenseTracker.Data;
using XpenseTracker.Helpers;
using XpenseTracker.Models;
using XpenseTracker.Services;

var builder = WebApplication.CreateBuilder(args);



//getting Database connection string from appsettings.json
//var connString = builder.Configuration.GetConnectionString("ApplicationDbContext");

//adding database connection, registering ApplicationDbContext with the dependency injection container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));


// Add services to the container.
builder.Services.AddControllers();

//registering the fluentvalidation service
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(); // or any known validator class

//registering the identity and authentication services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//registering mailkit service
builder.Services.AddScoped<MailHelper>();

// registering the expense service
builder.Services.AddScoped<IExpenseService, ExpenseService>();


var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseAuthentication();
app.UseAuthorization();

app.Run();
