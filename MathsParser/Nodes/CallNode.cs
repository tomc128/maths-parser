namespace MathsParser.Nodes;

public class CallNode : Node
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
                $"Invalid argument count for function {functionName}. Expected {methodParams.Length} argument(s), got {argCount}.");

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