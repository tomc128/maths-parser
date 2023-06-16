﻿namespace MathsParser;

public struct Number
{
    public const int Precision = 12;

    public double Value { get; }

    public int Numerator { get; }
    public int Denominator { get; }

    public bool DisplayAsFraction => Denominator != 1;


    public Number(double value)
    {
        Value = value;

        // Handle rounding to mitigate floating point errors
        if (Math.Abs(value - Math.Round(value)) < Math.Pow(10, -Precision)) Value = Math.Round(value, Precision);

        var fraction = Fractions.FromDouble(value);
        Numerator = fraction.numerator;
        Denominator = fraction.denominator;
    }

    public override string ToString()
    {
        // Remove out-of-range precision
        var rounded = Math.Round(Value, Precision);

        return DisplayAsFraction ? $"{Numerator}/{Denominator}" : $"{rounded}";
    }

    public string AsDecimal()
    {
        return $"{Value}";
    }
}