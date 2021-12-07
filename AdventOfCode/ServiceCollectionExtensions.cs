using AdventOfCode.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdventOfCodeServices(this IServiceCollection services)
            => services.AddTransient<IPuzzleInputSource, PuzzleInputSource>()
                       ;

        public static IServiceCollection AddPuzzle<T>(this IServiceCollection services) where T : class, IPuzzle
            => services.AddScoped<IPuzzle, T>()
                       .AddHostedService<Startup>()
                       ;
    }
}
