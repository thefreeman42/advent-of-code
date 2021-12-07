using AdventOfCode;
using AdventOfCode._2021;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", false)
            .Build();
    })
    .UseSerilog((context, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);

        const string outputTemplate =
            "[{Level:u3}] {Message:lj} {NewLine}{Exception}";
        config.WriteTo.Console(outputTemplate: outputTemplate);
    })
    .ConfigureServices((context, services) =>
    {
        services
            .AddAdventOfCodeServices()
            .AddPuzzle<_7_TreacheryOfWhales>()
            ;
    })
    .UseConsoleLifetime()
    ;

await host.Build().RunAsync();