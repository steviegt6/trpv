using System.Threading.Tasks;

using CliFx;

namespace Tomat.TRPV.CLI;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        return await new CliApplicationBuilder()
                    .SetTitle("trpv")
                    .SetDescription("terraria resource pack validator")
                    .AddCommandsFromThisAssembly()
                    .Build()
                    .RunAsync(args);
    }
}