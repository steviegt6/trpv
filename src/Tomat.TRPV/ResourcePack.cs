using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.Json;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

using Tomat.TRPV.Validation;

namespace Tomat.TRPV;

public sealed class ResourcePack(string path)
{
    private const string pack_json        = "pack.json";
    private const string icon_png         = "icon.png";
    private const string content_dir      = "Content";
    private const string images_dir       = "Images";
    private const string localization_dir = "Localization";
    private const string music_dir        = "Music";
    private const string sounds_dir       = "Sounds";

    private static readonly string[] allowed_localization_extensions = [".json", ".csv"];
    private static readonly string[] allowed_music_extensions        = [".wav", ".mp3", ".ogg"];
    private static readonly string[] allowed_sound_extensions        = [".xnb"];

    private static readonly string[] localization_codes = ["en-US", "de-DE", "it-IT", "fr-FR", "es-ES", "ru-RU", "zh-Hans", "pt-BR", "pl-PL"];

    public string Path => path;

    public ResourcePackManifest? Manifest { get; private set; }

    public IEnumerable<DiagnosticMessage> Diagnostics => diagnostics;

    private readonly List<DiagnosticMessage> diagnostics = [];

    public void AddMessage(DiagnosticMessage message)
    {
        diagnostics.Add(message);
    }

    public void ResolveManifest()
    {
        // TODO: We can support passing the manifest JSON I guess.
        if (File.Exists(Path))
        {
            Messages.TRPV0002.Add(this, null, null);
            return;
        }

        if (!Directory.Exists(Path))
        {
            Messages.TRPV0001.Add(this, null, null);
            return;
        }

        var manifestPath = System.IO.Path.Combine(Path, pack_json);
        if (!File.Exists(manifestPath))
        {
            Messages.TRPV0003.Add(this, null, null, manifestPath);
            return;
        }

        try
        {
            string? manifestName         = null;
            string? manifestAuthor       = null;
            string? manifestDescription  = null;
            int?    manifestVersionMajor = null;
            int?    manifestVersionMinor = null;

            var manifestText = File.ReadAllText(manifestPath);

            // We parse it manually so we can give rich errors upon failure.
            var manifestDoc = JsonDocument.Parse(
                manifestText,
                new JsonDocumentOptions
                {
                    // Vanilla Terraria permits comments in JSON and trailing
                    // commas since Newtonsoft.Json's default handling does.
                    CommentHandling     = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                }
            );

            // Manually get each property.
            // Minimal viable pack.json must include values for Name, Author,
            // and Description, as well as a default value for Version ({ }).
            // Versions CAN be negative numbers, though it's not recommended.
            // Null and undefined values crash the game.

            var hasName        = manifestDoc.RootElement.TryGetProperty("Name",        out var manifestDocName);
            var hasAuthor      = manifestDoc.RootElement.TryGetProperty("Author",      out var manifestDocAuthor);
            var hasDescription = manifestDoc.RootElement.TryGetProperty("Description", out var manifestDocDescription);
            var hasVersion     = manifestDoc.RootElement.TryGetProperty("Version",     out var manifestDocVersion);

            if (!hasName)
            {
                Messages.TRPV0006.Add(this, manifestPath, null, "'Name' is required");
            }

            if (!hasAuthor)
            {
                Messages.TRPV0006.Add(this, manifestPath, null, "'Author' is required");
            }

            if (!hasDescription)
            {
                Messages.TRPV0006.Add(this, manifestPath, null, "'Description' is required");
            }

            if (!hasVersion)
            {
                Messages.TRPV0006.Add(this, manifestPath, null, "'Version' is required");
            }

            // Name can be anything aside from null or undefined, but it should
            // be a string.
            if (hasName)
            {
                if (manifestDocName.ValueKind != JsonValueKind.String)
                {
                    Messages.TRPV0006.Add(this, manifestPath, null, "'Name' must be a string");
                }
                else
                {
                    manifestName = manifestDocName.GetString();
                }
            }

            // Author can be anything aside from null or undefined, but it
            // should be a string.
            if (hasAuthor)
            {
                if (manifestDocAuthor.ValueKind != JsonValueKind.String)
                {
                    Messages.TRPV0006.Add(this, manifestPath, null, "'Author' must be a string");
                }
                else
                {
                    manifestAuthor = manifestDocAuthor.GetString();
                }
            }

            // Description can be anything aside from null or undefined, but it
            // should be a string.
            if (hasDescription)
            {
                if (manifestDocDescription.ValueKind != JsonValueKind.String)
                {
                    Messages.TRPV0006.Add(this, manifestPath, null, "'Description' must be a string");
                }
                else
                {
                    manifestDescription = manifestDocDescription.GetString();
                }
            }

            // The version may be an empty object, but we will require it to
            // have a major and minor version that are explicitly specified as
            // it's a best practice.
            if (hasVersion)
            {
                if (manifestDocVersion.ValueKind != JsonValueKind.Object)
                {
                    Messages.TRPV0006.Add(this, manifestPath, null, "'Version' must be an object with integer properties 'major' and 'minor'");
                }
                else
                {
                    var hasMajor = manifestDocVersion.TryGetProperty("major", out var manifestDocVersionMajor);
                    var hasMinor = manifestDocVersion.TryGetProperty("minor", out var manifestDocVersionMinor);

                    if (!hasMajor)
                    {
                        Messages.TRPV0006.Add(this, manifestPath, null, "'Version' must have a 'major' property");
                    }

                    if (!hasMinor)
                    {
                        Messages.TRPV0006.Add(this, manifestPath, null, "'Version' must have a 'minor' property");
                    }

                    if (hasMajor)
                    {
                        if (manifestDocVersionMajor.ValueKind != JsonValueKind.Number)
                        {
                            Messages.TRPV0006.Add(this, manifestPath, null, "'Version.major' must be a number");
                        }
                        else
                        {
                            manifestVersionMajor = manifestDocVersionMajor.GetInt32();
                        }
                    }

                    if (hasMinor)
                    {
                        if (manifestDocVersionMinor.ValueKind != JsonValueKind.Number)
                        {
                            Messages.TRPV0006.Add(this, manifestPath, null, "'Version.minor' must be a number");
                        }
                        else
                        {
                            manifestVersionMinor = manifestDocVersionMinor.GetInt32();
                        }
                    }
                }
            }

            Manifest = new ResourcePackManifest(
                manifestName,
                manifestDescription,
                manifestAuthor,
                manifestVersionMajor,
                manifestVersionMinor
            );
        }
        catch (UnauthorizedAccessException)
        {
            Messages.TRPV0004.Add(this, null, null, manifestPath);
        }
        catch (SecurityException)
        {
            Messages.TRPV0004.Add(this, null, null, manifestPath);
        }
        catch (JsonException e)
        {
            Messages.TRPV0005.Add(this, manifestPath, ((int?)(e.LineNumber ?? null), null), e.Message);
        }

        Messages.TRPV0008.Add(
            this,
            manifestPath,
            null,
            Manifest?.Name,
            Manifest?.Name,
            Manifest?.Description?.Replace("\n", "\\n").Replace("\r", "\\r"),
            Manifest?.VersionMajor,
            Manifest?.VersionMinor
        );
    }

    public void ParseIcon()
    {
        var iconPath = System.IO.Path.Combine(Path, icon_png);
        if (!File.Exists(iconPath))
        {
            Messages.TRPV0010.Add(this, null, null, iconPath);
            return;
        }

        try
        {
            using var iconStream = File.OpenRead(iconPath);
            using var image      = Image.Load<Rgba32>(iconStream);

            // I'm lazy, and I am letting SixLabors determine what decoder to
            // use.  Technically, it can properly decode an icon.png that's
            // actually a different format such as JPEG.
            if (!image.Metadata.TryGetPngMetadata(out _))
            {
                Messages.TRPV0011.Add(this, null, null, iconPath);
            }

            // Assume the icon is fine at this point.
            // TODO: We can check for good sizes later.
            Messages.TRPV0012.Add(this, null, null, iconPath);
        }
        catch (UnauthorizedAccessException)
        {
            Messages.TRPV0013.Add(this, null, null, iconPath);
        }
        catch (InvalidImageContentException)
        {
            Messages.TRPV0011.Add(this, null, null, iconPath);
        }
        catch (UnknownImageFormatException)
        {
            Messages.TRPV0011.Add(this, null, null, iconPath);
        }
    }

    public void ParseContent()
    {
        var contentPath = System.IO.Path.Combine(Path, content_dir);
        if (!Directory.Exists(contentPath))
        {
            Messages.TRPV0007.Add(this, null, null, contentPath);
            return;
        }

        ParseImages(contentPath);
        ParseLocalization(contentPath);
        ParseMusic(contentPath);
        ParseSounds(contentPath);
    }

    private void ParseImages(string contentPath)
    {
        var imagesPath = System.IO.Path.Combine(contentPath, images_dir);
        if (!Directory.Exists(imagesPath))
        {
            Messages.TRPV0009.Add(this, null, null, imagesPath, false);
            return;
        }

        Messages.TRPV0009.Add(this, null, null, imagesPath, true);
    }

    private void ParseLocalization(string contentPath)
    {
        var localizationPath = System.IO.Path.Combine(contentPath, localization_dir);
        if (!Directory.Exists(localizationPath))
        {
            Messages.TRPV0009.Add(this, null, null, localizationPath, false);
            return;
        }

        Messages.TRPV0009.Add(this, null, null, localizationPath, true);

        foreach (var localizationFile in Directory.GetFiles(localizationPath, "*", SearchOption.AllDirectories))
        {
            var ext = System.IO.Path.GetExtension(localizationFile);

            if (!localization_codes.Any(x => System.IO.Path.GetFileName(localizationFile).StartsWith(x)))
            {
                Messages.TRPV2001.Add(this, localizationFile, null, '[' + string.Join(", ", localization_codes) + ']');
            }

            if (!allowed_localization_extensions.Contains(ext))
            {
                Messages.TRPV2002.Add(this, localizationFile, null, '[' + string.Join(", ", allowed_localization_extensions) + ']');
            }

            var properties = new List<string>();

            if (ext == ".json")
            {
                try
                {
                    var localizationDoc = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
                        File.ReadAllText(localizationFile),
                        new JsonSerializerOptions
                        {
                            ReadCommentHandling = JsonCommentHandling.Skip,
                            AllowTrailingCommas = true,
                        }
                    )!;
                    foreach (var (key, value) in localizationDoc)
                    {
                        foreach (var (subKey, _) in value)
                        {
                            properties.Add($"{key}.{subKey}");
                        }
                    }
                }
                catch (JsonException e)
                {
                    Messages.TRPV2003.Add(this, localizationFile, ((int?)(e.LineNumber ?? null), null), e.Message);
                }
            }
            else if (ext == ".csv")
            {
                var lines = File.ReadAllLines(localizationFile);
                for (var i = 1; i < lines.Length; i++)
                {
                    var line  = lines[i];
                    var parts = line.Split(',', 2);
                    if (parts.Length < 2)
                    {
                        Messages.TRPV2004.Add(this, localizationFile, (i + 1, null), $"Got one or less parts (length: {parts.Length})");
                    }
                    else
                    {
                        properties.Add(parts[0]);
                    }
                }
            }

            foreach (var property in properties)
            {
                if (!ContentDump.LocalizationKeys.Contains(property))
                {
                    Messages.TRPV2005.Add(this, localizationFile, null, property);
                }
            }
        }
    }

    private void ParseMusic(string contentPath)
    {
        const string music_name = "Music_";
        const int    music_min  = 1;

        var musicPath = System.IO.Path.Combine(contentPath, music_dir);
        if (!Directory.Exists(musicPath))
        {
            Messages.TRPV0009.Add(this, null, null, musicPath, false);
            return;
        }

        Messages.TRPV0009.Add(this, null, null, musicPath, true);

        var musicFiles = Directory.GetFiles(musicPath);
        foreach (var musicFile in musicFiles)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(musicFile);
            if (!fileName.StartsWith(music_name))
            {
                Messages.TRPV3001.Add(this, musicFile, null, music_min, ContentDump.MaxMusicId);
                continue;
            }

            if (!int.TryParse(fileName[music_name.Length..], out var musicId) || musicId < music_min || musicId >= ContentDump.MaxMusicId)
            {
                Messages.TRPV3001.Add(this, musicFile, null, music_min, ContentDump.MaxMusicId);
                continue;
            }

            if (!allowed_music_extensions.Contains(System.IO.Path.GetExtension(musicFile)))
            {
                Messages.TRPV3002.Add(this, musicFile, null, '[' + string.Join(", ", allowed_music_extensions) + ']');
            }

            // TODO: Add INFO diagnostic acknowledging validity?
            // TODO: Check file validity; are they corrupted?
        }
    }

    private void ParseSounds(string contentPath)
    {
        var soundsPath = System.IO.Path.Combine(contentPath, sounds_dir);
        if (!Directory.Exists(soundsPath))
        {
            Messages.TRPV0009.Add(this, null, null, soundsPath, false);
            return;
        }

        Messages.TRPV0009.Add(this, null, null, soundsPath, true);

        foreach (var soundFile in Directory.GetFiles(soundsPath, "*", SearchOption.AllDirectories))
        {
            var fileName = soundFile[(soundsPath.Length + 1)..];
            fileName = fileName.Replace('\\', '/');

            // I particularly hate this and wish the standard library had a way
            // to deal with this.  Get the file name minus the extension.  We
            // can't JUST use GetFileNameWithoutExtension because it doesn't
            // handle directories.  Instead, we gotta mash it together.  If
            // there is no directory, Combine behaves weird.  Ugh.
            var fileNameNoExt = System.IO.Path.GetDirectoryName(fileName) is { } dirName
                ? System.IO.Path.Combine(dirName, System.IO.Path.GetFileNameWithoutExtension(fileName))
                : System.IO.Path.GetFileNameWithoutExtension(fileName);
            fileNameNoExt = fileNameNoExt.Replace('\\', '/');

            if (!ContentDump.Sounds.Contains(fileNameNoExt))
            {
                Messages.TRPV4001.Add(this, soundFile, null);
                continue;
            }

            if (!allowed_sound_extensions.Contains(System.IO.Path.GetExtension(soundFile)))
            {
                Messages.TRPV4002.Add(this, soundFile, null, '[' + string.Join(", ", allowed_sound_extensions) + ']');
            }
        }
    }
}