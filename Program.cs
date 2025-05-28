

using Microsoft.EntityFrameworkCore;
using XpenseTracker.Data;

var builder = WebApplication.CreateBuilder(args);

//getting configuration from appsettings.json

//getting Database connection string from appsettings.json
//var connString = builder.Configuration.GetConnectionString("ApplicationDbContext");

//adding database connection, registering ApplicationDbContext with the dependency injection container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run();
