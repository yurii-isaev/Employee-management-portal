using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using WebAPI.Infrastructure.Data.Identity;
using static System.Reflection.Assembly;

namespace WebAPI.Presentation
{
    public static class Program
    {
        /// <summary>
        /// Program initialization.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static void Main(string[] args)
        {
            // Configure logging first.
            ConfigureLogging();

            var logger = NLogBuilder
                .ConfigureNLog("Nlog.config")
                .GetCurrentClassLogger();
            try
            {
                logger.Debug("Program initialization");

                #region Program initialization

                var host = CreateHostBuilder(args).Build();
                using var scope = host.Services
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

#pragma warning disable 4014
                ClaimRoleManager.Initializer(scope.ServiceProvider);
#pragma warning restore 4014

                #endregion

                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Program fall");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        #region Configure Logging

        /// <summary>
        /// Configure Logging.
        /// </summary>
        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build(); 
            
            // Create Logger
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        #endregion

        #region Configure Elasticsearch Sink
        
        /// <summary>
        /// Configure Elasticsearch Sink.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="env"></param>
        /// <returns>Options the elasticsearch sink</returns>
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot config, string env)
        {
            var sink = new ElasticsearchSinkOptions(new Uri(config["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat =
                $@"{GetExecutingAssembly()
                    .GetName().Name
                    .ToLower()
                    .Replace(".", "-")}-{env?.ToLower()
                    .Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
            return sink;
        }
        
        #endregion

        /// <summary>
        /// Create web host.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>web host</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureAppConfiguration(configuration =>
                {
                    configuration.AddJsonFile("appsettings.json", false, true);
                })
                .UseSerilog()
                .UseNLog(); // NLog: Setup NLog for Dependency injection.
    }
}