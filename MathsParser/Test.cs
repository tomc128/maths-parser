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
            { "π", Math.PI },
            { "e", Math.E },
        };

        var environment = new Environment(functions, variables);

        var parser = new Parser();

        var inputs = new[]
        {
            "3 pi",
            "2pi",
            "sin 2pi",
            "2 * 2",
            "ln e³",
            "4(2)",
            "clamp(4, 2, 3)",
            "2 * sin pi",
            "2 + 3 * 4",
            "abs -5",
            "sqrt 16 + 8",
            "tan(3pi/4)",
            "log(100, 10)",
            "2^3^2",
            "3(2 + 1)",
            "sqrt(9) + 1.5 * sin(pi/2)",
        };


        var compact = true;

        foreach (var input in inputs)
        {
            var node = parser.Read(input);
            var result = new Number(node.Evaluate(environment));

            var output = compact
                ? $"{input} -> {node} = {result.AsDecimal()} = {result}"
                : $"{input} -> {node} [root:{node.Type}]\n= {result.AsDecimal()} = {result}\n";

            Console.WriteLine(output);
        }
    }
}