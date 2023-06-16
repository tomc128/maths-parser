namespace MathsParser;

public static class Fractions
{
    public static (double numerator, double denominator) Simplify(double numerator, double denominator)
    {
        var gcd = GCD(numerator, denominator);
        return (numerator / gcd, denominator / gcd);
    }
    
    public static double GCD(double a, double b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }
    
    public static double LCM(double a, double b)
    {
        return a * b / GCD(a, b);
    }
    
    public static double FromDouble(double value, double error = 0.000001)
    {
        
    }
}