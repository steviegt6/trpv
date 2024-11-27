using System.Threading.Tasks;

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace Tomat.TRPV.CLI.Commands;

[Command(Description = "Validates a resource pack")]
public class ValidateCommand : ICommand
{
    private sealed class PathNotFoundResourcePack : ResourcePack
    {
        
    }
    
    [CommandParameter(0, Description = "Path to the resource pack or the Steam Workshop ID; defaults to current directory", IsRequired = false)]
    public string? ResourcePack { get; set; }

    ValueTask ICommand.ExecuteAsync(IConsole console)
    {
        var resourcePack = ResolveResourcePack(ResourcePack);
        
        return default(ValueTask);
    }

    private static ResourcePack ResolveResourcePack(string resourcePack)
    {
        if (long.TryParse(resourcePack, out var steamWorkshopId))
        {
            return ResolveSteamWorkshopResourcePack(steamWorkshopId);
        }
        
        return ResolveResourcePackFromPath(resourcePack);
    }

    private static ResourcePack ResolveResourcePackFromPath(string path)
    {
        
    }
}