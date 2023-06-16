namespace MathsParser;

public abstract class Node
{
    public abstract NodeType Type { get; }
}

internal class BinaryNode : Node
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
}

internal class CallNode : Node
{
    public CallNode(Node function, Node[] arguments)
    {
        Function = function;
        Arguments = arguments;
    }

    public override NodeType Type => NodeType.Call;

    public Node Function { get; }
    public Node[] Arguments { get; }
}

internal class NumberNode : Node
{
    public NumberNode(Token token)
    {
        Value = double.Parse(token.Value);
    }

    public override NodeType Type => NodeType.Number;

    public double Value { get; }
}

internal class IdentifierNode : Node
{
    public IdentifierNode(Token token)
    {
        Value = token.Value;
    }

    public override NodeType Type => NodeType.Identifier;

    public string Value { get; }
}

internal class ExpressionNode : Node
{
    public ExpressionNode(string expression)
    {
        Expression = expression;
    }

    public override NodeType Type => NodeType.Expression;

    public string Expression { get; }
}