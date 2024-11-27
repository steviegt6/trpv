using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Tomat.TRPV;

internal readonly record struct Dimensions(int Width, int Height);

internal static class ContentDump
{
    private sealed class Dump
    {
        // ReSharper disable MemberHidesStaticFromOuterClass
        public string[] LocalizationKeys { get; set; } = null!;

        public Dictionary<string, Dimensions> ImageDimensions { get; set; } = null!;

        public int MaxMusicId { get; set; }

        public string[] Sounds { get; set; } = null!;
        // ReSharper restore MemberHidesStaticFromOuterClass
    }

    public static string[] LocalizationKeys => dump.LocalizationKeys;

    public static Dictionary<string, Dimensions> ImageDimensions => dump.ImageDimensions;

    public static int MaxMusicId => dump.MaxMusicId;

    public static string[] Sounds => dump.Sounds;

    private static readonly Dump dump;

    static ContentDump()
    {
        using var dumpJsonStream = typeof(ContentDump).Assembly.GetManifestResourceStream("Tomat.TRPV.Resources.dump.json")!;
        using var dumpJsonReader = new StreamReader(dumpJsonStream);

        dump = JsonSerializer.Deserialize<Dump>(dumpJsonReader.ReadToEnd())!;
    }
}