namespace MathsParser;

public static class Fractions
{
    public static (int numerator, int denominator) Simplified(double numerator, double denominator)
    {
        var gcd = GCD(numerator, denominator);
        return ((int)(numerator / gcd), (int)(denominator / gcd));
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

    // Code based on https://stackoverflow.com/a/5128558
    public static (int numerator, int denominator) FromDouble(double value, double error = 12,
        double maxDenominator = 1_000_000)
    {
        error = Math.Pow(10, -error);

        var floor = (int)Math.Floor(value);
        value -= floor;

        if (value < error) return (floor, 1);
        if (1 - error < value) return (floor + 1, 1);

        var lowerNumerator = 0.0;
        var lowerDenominator = 1.0;

        var upperNumerator = 1.0;
        var upperDenominator = 1.0;

        var i = 0;

        while (true)
        {
            var middleNumerator = lowerNumerator + upperNumerator;
            var middleDenominator = lowerDenominator + upperDenominator;

            if (middleDenominator * (value + error) < middleNumerator)
            {
                upperNumerator = middleNumerator;
                upperDenominator = middleDenominator;
            }
            else if (middleNumerator < (value - error) * middleDenominator)
            {
                lowerNumerator = middleNumerator;
                lowerDenominator = middleDenominator;
            }
            else
            {
                // The fraction has too large of a denominator, return the decimal value
                if (middleDenominator > maxDenominator) return ((int)(floor + value), 1);

                // Return the fraction
                return ((int)(floor * middleDenominator + middleNumerator), (int)middleDenominator);
            }

            i++;

            // Add a check to break the loop if it doesn't converge after a certain number of iterations
            if (i >= 1000) return ((int)(floor + value), 1);
        }
    }
}