using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Serialized;
using AsmResolver.PE.DotNet.Cil;

var terrariaDir = args[0];
var gamePath    = Path.Combine(terrariaDir, "Terraria.exe");

// Data to dump.
var localizationKeys = new List<string>();
var imageDimensions  = new List<(string path, int width, int height)>();
var maxMusicId       = -1;
var sounds           = new List<string>();

var module = ModuleDefinition.FromFile(gamePath);
foreach (var resource in module.Resources)
{
    if (resource is not SerializedManifestResource)
    {
        continue;
    }

    switch (resource.Name)
    {
        case "Terraria.IO.Data.ResourcePacksDefaultInfo.tsv":
        case "Terraria.Localization.Content.en-US.Game.json":
        case "Terraria.Localization.Content.en-US.Items.json":
        case "Terraria.Localization.Content.en-US.json":
        case "Terraria.Localization.Content.en-US.Legacy.json":
        case "Terraria.Localization.Content.en-US.NPCs.json":
        case "Terraria.Localization.Content.en-US.Projectiles.json":
        case "Terraria.Localization.Content.en-US.Town.json":
            var data = resource.GetData()!;
            var text = new StreamReader(new MemoryStream(data)).ReadToEnd();

            var localization = resource.Name.Value.EndsWith(".json");
            if (localization)
            {
                // Find localization data.

                var json = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
                    text,
                    new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true,
                    }
                );
                if (json is null)
                {
                    continue;
                }

                foreach (var (key, value) in json)
                {
                    foreach (var (subKey, _) in value)
                    {
                        localizationKeys.Add($"{key}.{subKey}");
                    }
                }
            }
            else
            {
                // Find image dimension data.

                var lines = text.Split("\r\n");
                foreach (var line in lines)
                {
                    if (line == "Path\tWidth\tHeight")
                    {
                        continue;
                    }

                    var parts = line.Split('\t');
                    if (parts.Length < 3)
                    {
                        continue;
                    }

                    var path   = parts[0];
                    var width  = int.Parse(parts[1]);
                    var height = int.Parse(parts[2]);

                    imageDimensions.Add((path, width, height));
                }
            }
            break;
    }
}

// Find max music ID.
{
    var main      = module.TopLevelTypes.First(x => x.FullName == "Terraria.Main");
    var mainCctor = main.Methods.First(x => x.Name             == ".cctor");
    var cctorBody = (CilMethodBody)mainCctor.MethodBody!;

    var maxMusicStsfld = cctorBody.Instructions.First(x => x.OpCode == CilOpCodes.Stsfld && x.Operand is SerializedFieldDefinition field && field.Name == "maxMusic");
    var index          = cctorBody.Instructions.IndexOf(maxMusicStsfld);
    var ldci4          = cctorBody.Instructions[index - 1];

    maxMusicId = (sbyte)ldci4.Operand!;
}

// Find sounds.
{
    var soundDir   = Path.Combine(terrariaDir, "Content", "Sounds");
    var soundFiles = Directory.GetFiles(soundDir, "*.xnb", SearchOption.AllDirectories);

    sounds.AddRange(soundFiles.Select(x => x[(soundDir.Length + 1)..]));
}

// Dump data to a JSON file.
var dumpJson = JsonSerializer.Serialize(
    new
    {
        LocalizationKeys = localizationKeys,
        ImageDimensions  = imageDimensions.ToDictionary(x => x.path, x => new { Height = x.height, Width = x.width }),
        MaxMusicId       = maxMusicId,
        Sounds           = sounds.Select(x => x.Replace('\\', '/')[..^".xnb".Length]),
    },
    new JsonSerializerOptions
    {
        WriteIndented = true,
    }
);
await File.WriteAllTextAsync("dump.json", dumpJson);