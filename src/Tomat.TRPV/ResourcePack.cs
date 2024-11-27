using System.Collections.Generic;
using System.IO;

using Tomat.TRPV.Validation;

namespace Tomat.TRPV;

public sealed class ResourcePack(string path)
{
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
    }
}