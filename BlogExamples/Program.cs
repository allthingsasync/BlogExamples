using System.Data.Common;

enum TokenKind
{
    EndOfFileToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    NumberLiteralToken,
    UnknownToken,
}

class Token
{
    public string Text;
    public TokenKind Kind;
    public int StartLocation;
}

class Lexer
{
    private string source;
    private int currentPosition;

    public Lexer(string source)
    {
        this.source = source;
        currentPosition = 0;
    }

    private char Current()
    {
        return source[currentPosition];
    }

    private bool IsCurrentADigit()
    {
        if(currentPosition >= source.Length)
        {
            return false;
        }

        return char.IsDigit(source[currentPosition]);
    }

    private void MoveToNextChar()
    {
        currentPosition++;
    }

    public Token NextToken()
    {
        if(currentPosition >= source.Length)
        {
            return new Token
            {
                Text = "\0",
                Kind = TokenKind.EndOfFileToken,
                StartLocation = currentPosition,
            };
        }

        if(IsCurrentADigit())
        {
            var start = currentPosition;
            var text = "";
            while(IsCurrentADigit())
            {
                text += Current();
                MoveToNextChar();
            }

            return new Token
            {
                StartLocation = start,
                Text = text,
                Kind = TokenKind.NumberLiteralToken,
            };
        }

        TokenKind kind;
        string tokenText;

        switch (Current())
        {
            case '+':
                tokenText = "+";
                kind = TokenKind.PlusToken;
                break;
            case '-':
                tokenText = "-";
                kind = TokenKind.MinusToken;
                break;
            case '*':
                tokenText = "*";
                kind = TokenKind.StarToken;
                break;
            case '/':
                tokenText = "/";
                kind = TokenKind.SlashToken;
                break;

            default:
                tokenText = Current().ToString();
                kind = TokenKind.UnknownToken;
                break;
        }

        var position = currentPosition;
        MoveToNextChar();
        return new Token
        {
            Kind = kind,
            Text = tokenText,
            StartLocation = position
        };
    }
}

class Program
{
    public static void Main()
    {
        var lexer = new Lexer("123+345a*2b2-1");
        var tok = lexer.NextToken();

        while(tok.Kind != TokenKind.EndOfFileToken)
        {
            Console.WriteLine($"Kind = {tok.Kind}\ttext = {tok.Text}\tstart = {tok.StartLocation}\tlength = {tok.Text.Length + tok.StartLocation}");
            tok = lexer.NextToken();
        }
    }
}