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
            { "abs", new Func<double, double>(Math.Abs) } // Must be double not decimal
        };
        var variables = new Dictionary<string, double>
        {
            { "pi", Math.PI },
            { "e", Math.E }
        };

        var environment = new Environment(functions, variables);

        var parser = new Parser();

        var inputs = new[]
        {
            // "1+2.5",
            // "sin(pi)",
            // "sin pi/2",
            // "cos 0",
            // "asin sin pi", // TODO: check
            // "-2 * 3 + |-12 / 2|",
            "-1 + 1",
            "+2 - 2",
            "2 - -2",
            "3e2 + 1",
            "4.2e-2 + 2",
            "-4e+3"
            // "3 * pi",
            // "3pi"


            // "3 2",
            // "3x+4"

            // TODO "3 2" should not be valid
            // TODO "3(2)" should be parsed as "3 * 2", this should also work with "3(pi)" and better yet, "3pi"
            // "sigma(\"2x + 1\", 1, 10)" // TODO: 1 and 10 do not show up in the output
            // TODO: allow for +1 to be a valid number
        };

        foreach (var input in inputs)
        {
            var node = parser.Read(input);
            Console.WriteLine($"{input} -> [{node.Type}] {node}");
            var result = new Number(node.Evaluate(environment));

            Console.WriteLine($"= {result}");

            Console.WriteLine();
        }
    }
}