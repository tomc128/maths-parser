namespace MathsParser;

public class Parser
{
    private readonly Tokeniser tokeniser;
    private dynamic lookahead;

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

    private bool Match(params TokenType[] types)
    {
        return types.Any(Match);
    }

    private Node Expression()
    {
        return Addition();
    }

    private Node Addition()
    {
        var left = Call();

        while (Match(TokenType.Add, TokenType.Subtract)) left = new BinaryNode(Eat(lookahead.Type), left, Call());

        return left;
    }

    private Node Multiplication()
    {
        var left = Exponentiation();

        while (Match(TokenType.Multiply, TokenType.Divide))
            left = new BinaryNode(Eat(lookahead.Type), left, Exponentiation());

        return left;
    }

    private Node Exponentiation()
    {
        var left = Basic();

        while (Match(TokenType.Exponent))
            left = new BinaryNode(Eat(lookahead.Type), left, Basic());

        return left;
    }

    private Node Basic();

    private Node Call()
    {
        var callee = Multiplication();

        if (Match(TokenType.Number, TokenType.Identifier)) return new CallNode(callee, new[] { Call() });

        if (Match(TokenType.OpenBracket))
        {
            Eat(TokenType.OpenBracket);
            var args = new[] { Expression() };

            while (Match(TokenType.Comma))
            {
                Eat(TokenType.Comma);
                args.Append(Expression());
            }

            Eat(TokenType.CloseBracket);
            return new CallNode(callee, args);
        }

        return callee;
    }
}