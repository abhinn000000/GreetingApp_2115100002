using NLog.Web;
using NLog;
using NLog.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//logger using NLog
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
LogManager.Configuration = new XmlLoggingConfiguration("C:\\Users\\Lenovo\\source\\repos\\HelloGreetingApplication\\nlog.config");

logger.Debug("init main");

builder.Logging.ClearProviders();
builder.Host.UseNLog();
var app = builder.Build();

    //logger dependency injection

    //Add swagger to container

    app.UseSwagger();
    app.UseSwaggerUI();


    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
