namespace Tomat.TRPV.Validation;

/// <summary>
///     The severity of a message.
/// </summary>
public enum MessageSeverity
{
    /// <summary>
    ///     An informative message.
    /// </summary>
    Info,

    /// <summary>
    ///     A warning that indicates a problem but may not necessarily be an
    ///     error.
    /// </summary>
    Warn,

    /// <summary>
    ///     An error which indicates a problem that must be fixed.
    /// </summary>
    Error,
}