using TradingPlatform.Services;
using TradingPlatform.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register config globally
AppConfig.Init(builder.Configuration);


// 🔹 Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // read settings from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/app_log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // replace default logger

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IIndicatorService, IndicatorService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHttpClient<IOandaService, OandaService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "TradingPlatform API", Version = "v1" });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
