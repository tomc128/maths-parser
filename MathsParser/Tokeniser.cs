using System.Text.RegularExpressions;

namespace MathsParser;

public partial class Tokeniser
{
    private int index;
    private string input;
    public List<string> tokens;

    public void Read(string input)
    {
        this.input = input;
        index = 0;
    }

    public Token Next()
    {
        if (index >= input.Length) return new Token(TokenType.End, null);

        var substring = input[index..];

        var token = substring switch
        {
            _ when NumberRegex().IsMatch(substring) =>
                new Token(TokenType.Number, NumberRegex().Match(substring).Value),
            _ when IdentifierRegex().IsMatch(substring) =>
                new Token(TokenType.Identifier, IdentifierRegex().Match(substring).Value),
            _ when StringRegex().IsMatch(substring) =>
                new Token(TokenType.String, StringRegex().Match(substring).Value),
            _ when AddRegex().IsMatch(substring) => new Token(TokenType.Add, "+"),
            _ when SubtractRegex().IsMatch(substring) => new Token(TokenType.Subtract, "-"),
            _ when MultiplyRegex().IsMatch(substring) => new Token(TokenType.Multiply, "*"),
            _ when DivideRegex().IsMatch(substring) => new Token(TokenType.Divide, "/"),
            _ when ExponentRegex().IsMatch(substring) => new Token(TokenType.Exponent, "^"),
            _ when OpenBracketRegex().IsMatch(substring) => new Token(TokenType.OpenBracket, "("),
            _ when CloseBracketRegex().IsMatch(substring) => new Token(TokenType.CloseBracket, ")"),
            _ when CommaRegex().IsMatch(substring) => new Token(TokenType.Comma, ","),
            _ when char.IsWhiteSpace(substring[0]) => new Token(TokenType.Whitespace, substring[0].ToString()),
            _ => throw new Exception($"Unexpected character '{substring[0]}' at index {index}")
        };

        if (token.Value != null) index += token.Value.Length;

        if (token.Type == TokenType.Whitespace) return Next();

        return token;
    }


    [GeneratedRegex("^\\d+(\\.\\d+)?")]
    private static partial Regex NumberRegex();

    [GeneratedRegex("^[a-zA-Z_]\\w*")]
    private static partial Regex IdentifierRegex();

    [GeneratedRegex("^\"[^\"]*\"")]
    private static partial Regex StringRegex();

    [GeneratedRegex("^\\+")]
    private static partial Regex AddRegex();

    [GeneratedRegex("^-")]
    private static partial Regex SubtractRegex();

    [GeneratedRegex("^\\*")]
    private static partial Regex MultiplyRegex();

    [GeneratedRegex("^/")]
    private static partial Regex DivideRegex();

    [GeneratedRegex("^\\^")]
    private static partial Regex ExponentRegex();

    [GeneratedRegex("^\\(")]
    private static partial Regex OpenBracketRegex();

    [GeneratedRegex("^\\)")]
    private static partial Regex CloseBracketRegex();

    [GeneratedRegex("^,")]
    private static partial Regex CommaRegex();
}