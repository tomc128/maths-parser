namespace MathsParser;

public class Test
{
    public static void Main()
    {
        var functions = new Dictionary<string, Delegate>
        {
            { "sin", Math.Sin },
            { "cos", Math.Cos },
            { "tan", Math.Tan },
            { "asin", Math.Asin },
            { "acos", Math.Acos },
            { "atan", Math.Atan },
            { "abs", new Func<double, double>(Math.Abs) },
            { "sqrt", Math.Sqrt },
            { "clamp", new Func<double, double, double, double>(Math.Clamp) },
            { "log", new Func<double, double, double>(Math.Log) },
            { "ln", new Func<double, double>(x => Math.Log(x, Math.E)) },
        };

        var variables = new Dictionary<string, double>
        {
            { "pi", Math.PI },
            { "e", Math.E },
        };

        var environment = new Environment(functions, variables);

        var parser = new Parser();

        var inputs = new[]
        {
            "2 * 1",
            "ln e^2",
            "sin 2*pi",
            "2^3",
            "2²",

            // "clamp(4, 2, 3)", // TODO: 2 and 3 are not parsed
            // TODO "3 2" should not be valid, but "3 pi" should be. related below
            // TODO "3(2)" should be parsed as "3 * 2", this should also work with "3(pi)" and better yet, "3pi"
        };

        foreach (var input in inputs)
        {
            var node = parser.Read(input);
            Console.WriteLine($"{input} -> {node} [root:{node.Type}]");
            var result = new Number(node.Evaluate(environment));

            Console.WriteLine($"= {result.AsDecimal()} = {result}");
            Console.WriteLine();
        }
    }
}