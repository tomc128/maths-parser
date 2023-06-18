﻿namespace MathsParser;

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
            { "x", 2 },
        };

        var environment = new Environment(functions, variables);

        var parser = new Parser();

        var inputs = new[]
        {
            // "2^3^4",
            // "4% * 50",
            // "50% * 4",
            // "0.5 * 20%",
            "2-1",
            "+2-2",
            "+2--1",
        };


        var compact = true;

        foreach (var input in inputs)
        {
            var node = parser.Read(input);

            Console.Write($"{input} -> {node}");

            var result = new Number(node.Evaluate(environment));

            Console.WriteLine(compact ? $" = {result.AsDecimal()} = {result}" : $" [root:{node.Type}]\n= {result.AsDecimal()} = {result}\n");
        }
    }
}