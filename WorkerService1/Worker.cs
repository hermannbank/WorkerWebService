using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var host = new WebHostBuilder()
                      .UseKestrel(options =>
                      {
                         
                      })
                      .UseStartup<Startup>()

                      .ConfigureServices(configure =>
                      {
                          //configure.AddSingleton<AppOptions>(_options);
                      })
                      .ConfigureLogging(option => option.AddDebug().AddConsole())
                      .Build();


                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Test is  {time}", DateTimeOffset.Now);
                    await host.RunAsync(stoppingToken);
                }
            }
            catch (Exception) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Test is stopping");
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Test had error");
            }
        }
    }
}
