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
            "1+2.5",
            "2.5+3",
            "1.25*3",
            "sin(2)"
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