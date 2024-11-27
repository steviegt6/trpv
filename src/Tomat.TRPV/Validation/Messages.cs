using System;

namespace Tomat.TRPV.Validation;

public class DiagnosticWrapper(
    DiagnosticLevel            level,
    string                     code,
    Func<ResourcePack, string> getMessage
)
{
    public void Add(
        ResourcePack              resourcePack,
        string?                   filePath,
        (int? Line, int? Column)? location,
        params object?[]          args
    )
    {
        resourcePack.AddMessage(
            new DiagnosticMessage(
                level,
                code,
                string.Format(getMessage(resourcePack), args),
                filePath,
                location
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

    public static readonly DiagnosticWrapper TRPV0003 = new(
        DiagnosticLevel.Error,
        "TRPV0003",
        _ => "Resource pack manifest '{0}' does not exist"
    );

    public static readonly DiagnosticWrapper TRPV0004 = new(
        DiagnosticLevel.Error,
        "TRPV0004",
        _ => "Cannot access pack manifest '{0}'"
    );

    public static readonly DiagnosticWrapper TRPV0005 = new(
        DiagnosticLevel.Error,
        "TRPV0005",
        _ => "Pack manifest is a malformed JSON document: {0}"
    );

    public static readonly DiagnosticWrapper TRPV0006 = new(
        DiagnosticLevel.Error,
        "TRPV0006",
        _ => "Invalid pack manifest: {0}"
    );
}