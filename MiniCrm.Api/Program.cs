

using Microsoft.Extensions.Hosting;
using MiniCrm.Core.Utility;
using Serilog;
using StackExchange.Redis;
using System.Globalization;

//Log.Logger = new LoggerConfiguration()
//    .WriteTo(formatProvider: CultureInfo.InvariantCulture)
//    .CreateBootstrapLogger();
var corsPolicy = "CorsPolicy";

IConnectionMultiplexer? redisConnection = null;

try
{
    var builder = WebApplication.CreateBuilder(args);
    //const string serviceName = ThisAssembly.Info.Product;
    //const string buildVersion = ThisAssembly.Info.Version;
   // builder.Host.ConfigureLogging(serviceName, buildVersion);
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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

}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    if (redisConnection is not null)
    {
        await redisConnection.CloseAsync(true);
        await redisConnection.DisposeAsync();
    }
    await Log.CloseAndFlushAsync();
}
