namespace Tomat.TRPV.Validation;

public readonly record struct DiagnosticMessage(
    DiagnosticLevel           Level,
    string                    Code,
    string                    Message,
    string?                   FilePath,
    (int? Line, int? Column)? Location
);