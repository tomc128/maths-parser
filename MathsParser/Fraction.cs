namespace MathsParser;

public readonly record struct Fraction(double Numerator, double Denominator)
{
    public override string ToString()
    {
        return Denominator == 1.0 ? $"{Numerator}" : $"{Numerator}/{Denominator}";
    }
}