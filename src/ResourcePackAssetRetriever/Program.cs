using System.Collections.Generic;
using System.IO;

using AsmResolver.DotNet;
using AsmResolver.DotNet.Serialized;

var terrariaDir = args[0];
var gamePath    = Path.Combine(terrariaDir, "Terraria.exe");

var imageDimensions = new List<(string path, int width, int height)>();

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
                // Extract localization data.
            }
            else
            {
                // Extract image dimension data.

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

_ = imageDimensions;