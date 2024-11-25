namespace Tomat.TRPV.Validation;

/// <summary>
///     A validation result message; may be informative, indicate a warning, or
///     display an error.
/// </summary>
public interface IMessage
{
    MessageSeverity Severity { get; }

    MessageSeverity? OverridenSeverity { get; }

    MessageSeverity EffectiveSeverity => OverridenSeverity ?? Severity;
}