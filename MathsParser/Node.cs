namespace MathsParser;

public abstract class Node
{
    public abstract NodeType Type { get; }

    public abstract double Evaluate(Environment environment = null);
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

    public override double Evaluate(Environment environment = null)
    {
        var functionName = Function.ToString();
        if (!environment.Functions.ContainsKey(functionName))
            throw new Exception($"Function {Function} does not exist.");

        var function = environment.Functions[functionName];


        var args = Arguments.Select(arg => arg.Evaluate(environment)).ToArray();
        var argCount = args.Length;

        var methodParams = function.Method.GetParameters();
        if (methodParams.Length != argCount)
            throw new Exception(
                $"Invalid argument count for function {functionName}. Expected {argCount} argument(s).");

        var convertedArgs = methodParams.Zip(args, (param, arg) => Convert.ChangeType(arg, param.ParameterType))
            .ToArray();


        try
        {
            var result = function.DynamicInvoke(convertedArgs);
            if (result is double d) return d;
            throw new Exception($"Function {functionName} did not return a double.");
        }
        catch (Exception e)
        {
            throw new Exception($"Function {functionName} threw an exception.", e);
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

    public override double Evaluate(Environment environment = null)
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

    public override double Evaluate(Environment environment = null)
    {
        return environment.Variables[Value];
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

    public override double Evaluate(Environment environment = null)
    {
        return new Parser().Read(Expression).Evaluate(environment);
    }
}

internal class AbsNode : Node
{
    public AbsNode(Node expression)
    {
        Expression = expression;
    }

    public override NodeType Type => NodeType.Abs;

    public Node Expression { get; }

    public override string ToString()
    {
        return $"|{Expression}|";
    }

    public override double Evaluate(Environment environment = null)
    {
        return Math.Abs(Expression.Evaluate(environment));
    }
}