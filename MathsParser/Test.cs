﻿namespace MathsParser;

public class Test
{
    public static void Main()
    {
        var parser = new Parser();

        var inputs = new[]
        {
            "1+2",
            "3 + 4",
            "7 * 8 + 9",
            "(8 + 2) *11",
            "1 + 3/4",
            "(1+3) / 4"
        };

        foreach (var input in inputs)
        {
            var node = parser.Read(input);
            Console.WriteLine($"{input} -> {node}");
        }
    }
}