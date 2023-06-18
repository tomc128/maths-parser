namespace MathsParser.Nodes;

public class BinaryNode : Node
{
    public BinaryNode(TokenType @operator, Node left, Node right)
    {
        Left = left;
        Right = right;
        Operator = @operator;
    }

    public override NodeType Type => NodeType.Binary;

    public Node Left { get; }
    public Node Right { get; }
    public TokenType Operator { get; }

    public override string ToString()
    {
        return $"({Left} {Operator} {Right})";
    }

    public override double Evaluate(Environment environment = null)
    {
        var left = Left.Evaluate(environment);
        var right = Right.Evaluate(environment);

        return Operator switch
        {
            TokenType.Add => left + right,
            TokenType.Subtract => left - right,
            TokenType.Multiply => left * right,
            TokenType.Divide => left / right,
            TokenType.Exponent => Math.Pow(left, right),
            _ => throw new Exception($"Unexpected operator {Operator}"),
        };
    }
}