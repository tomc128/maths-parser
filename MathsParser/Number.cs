namespace MathsParser;

public struct Number
{
    public double Value { get; }
    public Fraction Fraction { get; }

    public bool IsInteger => Value % 1 == 0;


    public Number(double value)
    {
        Value = value;
        Fraction = Fractions.FromDouble(value);
    }

    public override string ToString()
    {
        return Fraction.Denominator == 1 ? Value.ToString() : Fraction.ToString();
    }
}