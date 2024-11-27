using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text.Json;

using Tomat.TRPV.Validation;

namespace Tomat.TRPV;

public sealed class ResourcePack(string path)
{
    private const string pack_json = "pack.json";

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
            var manifestDoc = JsonDocument.Parse(manifestText);

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
    }
}