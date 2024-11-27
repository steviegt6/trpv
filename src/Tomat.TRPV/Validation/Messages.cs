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
    // 0000 - General diagnostics
    
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

    public static readonly DiagnosticWrapper TRPV0007 = new(
        DiagnosticLevel.Error,
        "TRPV0007",
        _ => "Resource pack does not have Content folder '{0}'"
    );

    public static readonly DiagnosticWrapper TRPV0008 = new(
        DiagnosticLevel.Info,
        "TRPV0008",
        _ => "Parsed resource pack metadata; name: '{0}', author: '{1}', description: '{2}', version: {3}.{4}"
    );

    public static readonly DiagnosticWrapper TRPV0009 = new(
        DiagnosticLevel.Info,
        "TRPV0009",
        _ => "{0} directory found: '{1}'"
    );

    public static readonly DiagnosticWrapper TRPV0010 = new(
        DiagnosticLevel.Warn,
        "TRPV0010",
        _ => "Icon '{0}' not found"
    );

    public static readonly DiagnosticWrapper TRPV0011 = new(
        DiagnosticLevel.Error,
        "TRPV0011",
        _ => "Icon '{0}' is not a valid PNG file"
    );

    public static readonly DiagnosticWrapper TRPV0012 = new(
        DiagnosticLevel.Info,
        "TRPV0012",
        _ => "Icon '{0}' is a valid PNG file"
    );
    
    public static readonly DiagnosticWrapper TRPV0013 = new(
        DiagnosticLevel.Error,
        "TRPV0013",
        _ => "Cannot access icon '{0}'"
    );
    
    // 1000 - Images diagnostics
    
    // 2000 - Localization diagnostics
    
    // 3000 - Music diagnostics
    
    public static readonly DiagnosticWrapper TRPV3001 = new(
        DiagnosticLevel.Error,
        "TRPV3001",
        _ => "Music file has an invalid name; must be Music_X where X is a number greater than or equal to {0} and less than {1}"
    );
    
    public static readonly DiagnosticWrapper TRPV3002 = new(
        DiagnosticLevel.Error,
        "TRPV3002",
        _ => "Music file has an invalid extension; allowed extensions: {0}"
    );
    
    // 4000 - Sounds diagnostics
    
    public static readonly DiagnosticWrapper TRPV4001 = new(
        DiagnosticLevel.Error,
        "TRPV4001",
        _ => "Unrecognized sound file"
    );
    
    public static readonly DiagnosticWrapper TRPV4002 = new(
        DiagnosticLevel.Error,
        "TRPV4002",
        _ => "Sound file has an invalid extension; allowed extensions: {0}"
    );
}