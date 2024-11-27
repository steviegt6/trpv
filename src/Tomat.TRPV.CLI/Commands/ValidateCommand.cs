using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

using Spectre.Console;

using Tomat.TRPV.Validation;

namespace Tomat.TRPV.CLI.Commands;

[Command(Description = "Validates a resource pack")]
public class ValidateCommand : ICommand
{
    [CommandParameter(0, Description = "Path to the resource pack or the Steam Workshop ID; defaults to current directory", IsRequired = false)]
    public string? ResourcePack { get; set; }

    ValueTask ICommand.ExecuteAsync(IConsole console)
    {
        ResourcePack ??= Directory.GetCurrentDirectory();

        var sw = new Stopwatch();
        sw.Start();
        var resourcePack = ResolveResourcePack(ResourcePack);
        {
            resourcePack.ResolveManifest();
        }
        sw.Stop();

        WriteDiagnostics(resourcePack, sw.Elapsed);
        return default(ValueTask);

        static void WriteDiagnostics(
            ResourcePack resourcePack,
            TimeSpan     elapsed
        )
        {
            var elapsedFormatted = elapsed.TotalSeconds < 1
                ? $"{elapsed.TotalMilliseconds}ms"
                : $"{elapsed.TotalSeconds}s";

            var rpName      = resourcePack.Manifest?.Name ?? "<unknown>";
            var failed      = resourcePack.Diagnostics.Any(x => x.Level == DiagnosticLevel.Error);
            var statusColor = failed ? "red" : "green";

            var errorCount = resourcePack.Diagnostics.Count(x => x.Level == DiagnosticLevel.Error);
            var warnCount  = resourcePack.Diagnostics.Count(x => x.Level == DiagnosticLevel.Warn);

            AnsiConsole.MarkupLine($"[white]{rpName} [{statusColor}]{(failed ? "failed" : "succeeded")} with {errorCount} error(s) and {warnCount} warning(s)[/] ({elapsedFormatted})[/]");
            {
                foreach (var warning in resourcePack.Diagnostics.Where(x => x.Level == DiagnosticLevel.Warn))
                {
                    PrintDiagnostic(warning);
                }

                foreach (var error in resourcePack.Diagnostics.Where(x => x.Level == DiagnosticLevel.Error))
                {
                    PrintDiagnostic(error);
                }
            }
        }

        static void PrintDiagnostic(DiagnosticMessage message)
        {
            var levelColor = message.Level switch
            {
                DiagnosticLevel.Info  => "white",
                DiagnosticLevel.Warn  => "yellow",
                DiagnosticLevel.Error => "red",
                _                     => throw new ArgumentOutOfRangeException(),
            };

            var levelName = message.Level.ToString().ToLowerInvariant();

            var sourceFile = message.FilePath ?? "";
            var lineNumber = message.LineNumber.HasValue ? $" (line {message.LineNumber})" : "";
            var location   = sourceFile + lineNumber;
            if (location.Length > 0)
            {
                // TODO: highlight file name in white
                AnsiConsole.Markup($"    [silver]{location}[white][/][/]: ");
            }
            else
            {
                AnsiConsole.Markup("    ");
            }

            AnsiConsole.Markup($"[{levelColor}]{levelName} {message.Code}: [silver]{message.Message}[/][/]");
        }
    }

    private static ResourcePack ResolveResourcePack(string resourcePack)
    {
        if (long.TryParse(resourcePack, out var steamWorkshopId))
        {
            return ResolveSteamWorkshopResourcePack(steamWorkshopId);
        }

        return ResolveResourcePackFromPath(resourcePack);
    }

    private static ResourcePack ResolveSteamWorkshopResourcePack(long steamWorkshopId)
    {
        throw new NotImplementedException("Steam Workshop support is not implemented yet");
    }

    private static ResourcePack ResolveResourcePackFromPath(string path)
    {
        return new ResourcePack(path);
    }
}