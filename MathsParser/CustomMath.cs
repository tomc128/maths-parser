namespace MathsParser;

public static class CustomMath
{
    public static double Factorial(double n)
    {
        if (n <= 1) return 1;
        return n * Factorial(n - 1);
    }
}