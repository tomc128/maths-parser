namespace MathsParser;

public class Parser
{
    private dynamic lookahead;
    private readonly Tokeniser tokeniser;

    public Parser()
    {
        tokeniser = new Tokeniser();
    }

    public void Read(string input)
    {
        tokeniser.Read(input);
        lookahead = tokeniser.Next();
    }

    private Token Eat(TokenType type)
    {
        var token = lookahead;

        if (token is null) throw new Exception($"Unexpected end of input, expected {type}");

        if (token.Type != type) throw new Exception($"Unexpected token {token}, expected {type}");

        lookahead = tokeniser.Next();

        return token;
    }
    
    private bool Match(TokenType type)
    {
        if (lookahead is null) return false;
        if (lookahead.Type != type) return false;
        return true;
    }
}