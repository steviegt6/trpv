namespace Tomat.TRPV;

public readonly record struct ResourcePackManifest(
    string? Name,
    string? Description,
    string? Author,
    int?    VersionMajor,
    int?    VersionMinor
);