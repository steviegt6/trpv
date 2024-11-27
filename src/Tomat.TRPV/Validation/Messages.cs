using System;

namespace Tomat.TRPV.Validation;

public class DiagnosticWrapper(
    DiagnosticLevel            level,
    string                     code,
    Func<ResourcePack, string> getMessage
)
{
    public void Add(
        ResourcePack resourcePack,
        string?      filePath,
        int?         lineNumber
    )
    {
        resourcePack.AddMessage(
            new DiagnosticMessage(
                level,
                code,
                getMessage(resourcePack),
                filePath,
                lineNumber
            )
        );
    }
}

public static class Messages
{
    public static readonly DiagnosticWrapper TRPV0001 = new(
        DiagnosticLevel.Error,
        "TRPV0001",
        pack => $"Resource pack directory '{pack.Path}' does not exist"
    );

    public static readonly DiagnosticWrapper TRPV0002 = new(
        DiagnosticLevel.Error,
        "TRPV0002",
        pack => $"Resource pack directory '{pack.Path}' is not a directory"
    );
}