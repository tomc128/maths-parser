namespace MathsParser;

public abstract class Node
{
    public abstract NodeType Type { get; }

    public abstract double Evaluate(dynamic environment = null);
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

    public override string ToString()
    {
        return $"({Left} {Operator} {Right})";
    }

    public override double Evaluate(dynamic environment = null)
    {
        var left = Left.Evaluate();
        var right = Right.Evaluate();

        return Operator switch
        {
            TokenType.Add => left + right,
            TokenType.Subtract => left - right,
            TokenType.Multiply => left * right,
            TokenType.Divide => left / right,
            TokenType.Exponent => Math.Pow(left, right),
            _ => throw new Exception($"Unexpected operator {Operator}")
        };
    }
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

    public override string ToString()
    {
        var args = "";
        foreach (var arg in Arguments) args += $"{arg}, ";
        var argsString = args.Length > 0 ? args[..^2] : "";
        return $"{Function}({argsString})";
    }

    public override double Evaluate(dynamic environment = null)
    {
        var function = environment[Function.ToString()];
        var args = Arguments.Select(arg => arg.Evaluate(environment)).ToArray();

        switch (args.Length)
        {
            case 0:
                return function();
            case 1:
                return function(args[0]);
            case 2:
                return function(args[0], args[1]);
            case 3:
                return function(args[0], args[1], args[2]);
            case 4:
                return function(args[0], args[1], args[2], args[3]);
            default:
                throw new Exception("Too many arguments. Current limit is 4.");
        }
    }
}

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

    public override double Evaluate(dynamic environment = null)
    {
        return Value;
    }
}

internal class IdentifierNode : Node
{
    public IdentifierNode(Token token)
    {
        Value = token.Value;
    }

    public override NodeType Type => NodeType.Identifier;

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    public override double Evaluate(dynamic environment = null)
    {
        return environment[Value];
    }
}

internal class ExpressionNode : Node
{
    public ExpressionNode(string expression)
    {
        Expression = expression;
    }

    public override NodeType Type => NodeType.Expression;

    public string Expression { get; }

    public override string ToString()
    {
        return $"\"{Expression}\"";
    }

    public override double Evaluate(dynamic environment = null)
    {
        return new Parser().Read(Expression).Evaluate(environment);
    }
}