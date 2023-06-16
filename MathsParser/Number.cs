namespace MathsParser;

public struct Number
{
    public double Value { get; }

    public int Numerator { get; }
    public int Denominator { get; }

    public bool DisplayAsFraction => Denominator != 1;


    public Number(double value)
    {
        Value = value;

        var fraction = Fractions.FromDouble(value);
        Numerator = fraction.numerator;
        Denominator = fraction.denominator;
    }

    public override string ToString() => DisplayAsFraction ? $"{Numerator}/{Denominator}" : Value.ToString();
}