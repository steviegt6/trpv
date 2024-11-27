using System.Collections.Generic;

using Tomat.TRPV.Validation;

namespace Tomat.TRPV;

public abstract class ResourcePack
{
    public IEnumerable<Message> Messages => messages;

    private readonly List<Message> messages = [];

    public virtual void AddMessage(
        MessageSeverity  severity,
        MessageKind      kind,
        string           text,
        MessageSeverity? overridenSeverity = null
    )
    {
        messages.Add(
            new Message
            {
                Severity          = severity,
                OverridenSeverity = overridenSeverity,
                Kind              = kind,
                Text              = text,
            }
        );
    }
}