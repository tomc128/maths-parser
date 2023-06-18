namespace MathsParser;

public enum TokenType
{
    End,
    Whitespace,
    UnsignedNumber,
    SignedNumber,
    Identifier,
    String,
    Add,
    Subtract,
    Multiply,
    Divide,
    Exponent,
    Percent,
    OpenBracket,
    CloseBracket,
    Abs,
    Sqrt,
    Comma,
}