namespace MathsParser;

public class Test
{
    public static void Main()
    {
        var tokeniser = new Tokeniser();
        tokeniser.Read("1 + 2");


        while (true)
        {
            var token = tokeniser.Next();
            Console.WriteLine(token);
            if (token.Type == TokenType.End) break;
        }
    }
}