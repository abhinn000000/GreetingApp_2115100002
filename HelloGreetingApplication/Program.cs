using NLog.Web;
using NLog;
using NLog.Config;
using BusinessLayer.Interface;
using BusinessLayer.Services;
using RepositoryLayer.Services;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//database connectivity
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<GreetingContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL, GreetingRL>();


//logger using NLog
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
LogManager.Configuration = new XmlLoggingConfiguration("C:\\Users\\Lenovo\\source\\repos\\HelloGreetingApplication\\nlog.config");

logger.Debug("init main");

builder.Logging.ClearProviders();
builder.Host.UseNLog();
var app = builder.Build();



//Add swagger to container

app.UseSwagger();
    app.UseSwaggerUI();


    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
