namespace MathsParser.Nodes;

public class IdentifierNode : Node
{
    public IdentifierNode(Token token)
    {
        Value = token.Value;
    }

    public IdentifierNode(string value)
    {
        Value = value;
    }

    public override NodeType Type => NodeType.Identifier;

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    public override double Evaluate(Environment environment = null)
    {
        if (!environment.Variables.ContainsKey(Value)) throw new Exception($"Variable {Value} does not exist.");
        return environment.Variables[Value];
    }
}