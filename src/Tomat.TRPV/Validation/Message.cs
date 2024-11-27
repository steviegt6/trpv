namespace Tomat.TRPV.Validation;

/// <summary>
///     A validation result message; may be informative, indicate a warning, or
///     display an error.
/// </summary>
public readonly struct Message
{
    /// <summary>
    ///     The actual message severity.
    /// </summary>
    public MessageSeverity Severity { get; init; }

    /// <summary>
    ///     The overriden message severity, if specified.
    /// </summary>
    public MessageSeverity? OverridenSeverity { get; init; }

    /// <summary>
    ///     The effective message severity, which is the overriden severity if
    ///     specified, or the actual severity otherwise.
    /// </summary>
    public MessageSeverity EffectiveSeverity => OverridenSeverity ?? Severity;

    /// <summary>
    ///     The message kind.
    /// </summary>
    public MessageKind Kind { get; init; }

    /// <summary>
    ///     The message text.
    /// </summary>
    public string Text { get; init; }
}