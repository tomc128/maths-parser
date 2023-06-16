namespace MathsParser;

public class Test
{
    public static void Main()
    {
        var environment = new Dictionary<string, Delegate>
        {
            { "sin", Math.Sin },
            { "cos", Math.Cos },
            { "tan", Math.Tan }
        };

        var parser = new Parser();

        var inputs = new[]
        {
            "1+2",
            "3 + 4",
            "7 * 8 + 9",
            "(8 + 2) *11",
            "1 + 3/4",
            "(1+3) / 4",
            "2^3 + 1",
            "3 * {2 + (1 / 3)}",
            "-3 + 2",
            "2 + 1",
            "sin(2)"
            // "sin pi",
            // "sin 2 pi",
            // "sigma(\"2x + 1\", 1, 10)" // TODO: 1 and 10 do not show up in the output
            // TODO: allow for +1 to be a valid number
        };

        foreach (var input in inputs)
        {
            var node = parser.Read(input);
            Console.WriteLine($"{input} -> [{node.Type}] {node}");
            Console.WriteLine($"  = {node.Evaluate(environment)}");

            Console.WriteLine();
        }
    }
}