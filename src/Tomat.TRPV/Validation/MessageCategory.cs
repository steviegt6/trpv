namespace Tomat.TRPV.Validation;

public readonly record struct MessageKind(string Category, int Id)
{
    public override string ToString()
    {
        return $"{Category}{Id:0000}";
    }
}