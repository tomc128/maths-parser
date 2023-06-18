using MathsParser.Nodes;

namespace MathsParser;

public class Parser
{
    private readonly Tokeniser tokeniser;
    private Token lookahead;

    public Parser()
    {
        tokeniser = new Tokeniser();
    }

    public Node Read(string input)
    {
        tokeniser.Read(input);
        lookahead = tokeniser.Next();
        return Expression();
    }

    private Token Eat(TokenType type)
    {
        var token = lookahead;

        if (token.Type == TokenType.End) throw new Exception($"Unexpected end of input, expected {type}");
        if (token.Type != type) throw new Exception($"Unexpected token {token}, expected {type}");

        lookahead = tokeniser.Next();

        return token;
    }

    private Token Eat(params TokenType[] types)
    {
        var token = lookahead;

        if (token.Type == TokenType.End) throw new Exception($"Unexpected end of input, expected {string.Join(" or ", types)}");
        if (!types.Contains(token.Type)) throw new Exception($"Unexpected token {token}, expected {string.Join(" or ", types)}");

        lookahead = tokeniser.Next();

        return token;
    }

    private bool Match(TokenType type)
    {
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

        while (Match(TokenType.Add, TokenType.Subtract, TokenType.SignedNumber))
            if (lookahead.Type == TokenType.SignedNumber)
            {
                var next = Eat(lookahead.Type);
                var sign = next.Value[0];
                var abs = next.Value[1..];

                var operation = sign == '+' ? TokenType.Add : TokenType.Subtract;
                // If there is another token, do a Call, otherwise just return a NumberNode
                left = new BinaryNode(operation, left, Match(TokenType.End) ? new NumberNode(abs) : Call());
            }
            else
            {
                left = new BinaryNode(Eat(lookahead.Type).Type, left, Call());
            }

        return left;
    }

    private Node Multiplication()
    {
        var left = Exponentiation();

        while (Match(TokenType.Multiply, TokenType.Divide))
            left = new BinaryNode(Eat(lookahead.Type).Type, left, Call()); // TODO: check if changing from Exponentiation -> Call is correct

        return left;
    }

    private Node Exponentiation()
    {
        var left = Basic();

        while (Match(TokenType.Exponent))
        {
            var next = Eat(lookahead.Type);

            if (next.Value == "^")
            {
                // Evaluate the right-hand side of the operator first, as exponentiation is right-associative
                var right = Exponentiation();
                left = new BinaryNode(next.Type, left, right);
            }
            else
            {
                left = next.Value switch
                {
                    "⁰" => new BinaryNode(next.Type, left, new NumberNode(0)),
                    "¹" => new BinaryNode(next.Type, left, new NumberNode(1)),
                    "²" => new BinaryNode(next.Type, left, new NumberNode(2)),
                    "³" => new BinaryNode(next.Type, left, new NumberNode(3)),
                    "⁴" => new BinaryNode(next.Type, left, new NumberNode(4)),
                    "⁵" => new BinaryNode(next.Type, left, new NumberNode(5)),
                    "⁶" => new BinaryNode(next.Type, left, new NumberNode(6)),
                    "⁷" => new BinaryNode(next.Type, left, new NumberNode(7)),
                    "⁸" => new BinaryNode(next.Type, left, new NumberNode(8)),
                    "⁹" => new BinaryNode(next.Type, left, new NumberNode(9)),
                    _ => throw new Exception("Invalid exponent value"),
                };
            }
        }

        return left;
    }

    private Node Basic()
    {
        if (Match(TokenType.OpenBracket))
        {
            Eat(TokenType.OpenBracket);
            var expression = Expression();
            Eat(TokenType.CloseBracket);

            return expression;
        }

        if (Match(TokenType.Abs))
        {
            Eat(TokenType.Abs);
            var expression = Expression();
            Eat(TokenType.Abs);

            return new AbsCallNode(new[] { expression });
        }


        if (Match(TokenType.UnsignedNumber, TokenType.SignedNumber))
        {
            var number = Eat(TokenType.UnsignedNumber, TokenType.SignedNumber);

            // check for percent
            if (Match(TokenType.Percent))
            {
                Eat(TokenType.Percent);
                return new BinaryNode(TokenType.Divide, new NumberNode(number), new NumberNode(100));
            }

            return new NumberNode(number);
        }

        if (Match(TokenType.Identifier)) return new IdentifierNode(Eat(TokenType.Identifier));

        if (Match(TokenType.String))
        {
            var expression = Eat(TokenType.String).Value?[1..^1];
            if (expression is null) throw new Exception("String value is null");

            new Parser().Read(expression);

            return new ExpressionNode(expression);
        }

        throw new Exception($"Unexpected token {lookahead}");
    }

    private Node Call()
    {
        var callee = Multiplication();

        var isMultiplication = false;

        // only allow multiplication if the next token is an identifier and callee is not (this is not a function call)
        if (callee is not IdentifierNode)
        {
            // However, if callee is a number and the next token is a bracket, this should be treated as a multiplication
            if (callee is NumberNode && Match(TokenType.OpenBracket))
                // So we go to the bracket check below
            {
                isMultiplication = true;
            }
            else
            {
                // An identifier next would indicate multiplication, like "2x" 
                if (Match(TokenType.Identifier)) return new BinaryNode(TokenType.Multiply, callee, Multiplication());

                // Anything but a signed number next would indicate the end of the expression
                if (!Match(TokenType.SignedNumber)) return callee;

                // The next token is a number with a +/-, so it's an addition/subtraction instead of a multiplication
                // Return either an addition or subtraction node
                var next = Eat(TokenType.SignedNumber);
                var sign = next.Value[0];
                return new BinaryNode(sign == '+' ? TokenType.Add : TokenType.Subtract, callee, new NumberNode(next.Value[1..]));
            }
        }

        if (Match(TokenType.UnsignedNumber, TokenType.SignedNumber, TokenType.Identifier)) return new CallNode(callee, new[] { Call() });

        if (Match(TokenType.OpenBracket))
        {
            Eat(TokenType.OpenBracket);
            var args = new List<Node> { Expression() };

            if (isMultiplication && Match(TokenType.Comma))
                throw new Exception("Unexpected comma in implicit bracketed multiplication");

            while (Match(TokenType.Comma))
            {
                Eat(TokenType.Comma);
                args.Add(Expression());
            }

            Eat(TokenType.CloseBracket);

            // return new CallNode(callee, args);
            return isMultiplication ? new BinaryNode(TokenType.Multiply, callee, args[0]) : new CallNode(callee, args.ToArray());
        }

        return callee;
    }
}