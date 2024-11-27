using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

var items = new List<(string path, int width, int height)>();

var terrariaDir = args[0];

var gameAsmPath = Path.Combine(terrariaDir, "Terraria.exe");

using var asmStream   = File.OpenRead(gameAsmPath);
using var asmPeReader = new PEReader(asmStream);

var metadataReader = asmPeReader.GetMetadataReader();

foreach (var entryHandle in metadataReader.ManifestResources)
{
    var resource     = metadataReader.GetManifestResource(entryHandle);
    var resourceName = metadataReader.GetString(resource.Name);

    if (!resource.Implementation.IsNil)
    {
        continue;
    }

    var resOffset    = asmPeReader.PEHeaders.CorHeader!.ResourcesDirectory.RelativeVirtualAddress;
    var resourceData = asmPeReader.GetSectionData(resOffset + (int)resource.Offset).GetReader();

    switch (resourceName)
    {
        case "Terraria.IO.Data.ResourcePacksDefaultInfo.tsv":
        case "Terraria.Localization.Content.en-US.Game.json":
        case "Terraria.Localization.Content.en-US.Items.json":
        case "Terraria.Localization.Content.en-US.json":
        case "Terraria.Localization.Content.en-US.Legacy.json":
        case "Terraria.Localization.Content.en-US.NPCs.json":
        case "Terraria.Localization.Content.en-US.Projectiles.json":
        case "Terraria.Localization.Content.en-US.Town.json":
        {
            using var resourceStream = new MemoryStream(resourceData.ReadBytes(resourceData.Length));
            using var stringReader   = new StreamReader(resourceStream);
            var       text           = stringReader.ReadToEnd();

            var isLocalization = resourceName.EndsWith(".json");

            if (isLocalization)
            {
                // Export properties from localization files.
            }
            else
            {
                // Export item dimensions from ResourcePacksDefaultInfo.tsv.

                var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts  = line.Split('\t');
                    var name   = parts[0];
                    var width  = int.Parse(parts[1]);
                    var height = int.Parse(parts[2]);

                    items.Add((name, width, height));
                }
            }

            break;
        }
    }
}

_ = items;