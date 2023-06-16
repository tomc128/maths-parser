namespace MathsParser;

public abstract class Node
{
    public abstract NodeType Type { get; }

    public abstract double Evaluate(Environment environment = null);
}