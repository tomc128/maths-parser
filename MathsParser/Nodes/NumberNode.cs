namespace MathsParser.Nodes;

internal class NumberNode : Node
{
    public NumberNode(Token token)
    {
        Value = double.Parse(token.Value);
    }

    public override NodeType Type => NodeType.Number;

    public double Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override double Evaluate(Environment environment = null)
    {
        return Value;
    }
}