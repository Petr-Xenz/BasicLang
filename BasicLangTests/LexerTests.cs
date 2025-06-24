using Newtonsoft.Json.Linq;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Tests;

[TestClass]
public class LexerTests
{
    private static readonly int _newLineLength = Environment.NewLine.Length;

    [TestMethod]
    public void IntegerLexing()
    {
        var source = "1 23 023 14778";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new(Number, "1", SourcePosition.Empty),
            new(Number, "23", SourcePosition.Empty),
            new(Number, "023", SourcePosition.Empty),
            new(Number, "14778", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void FloatLexing()
    {
        var source = "1.23 023.14778";
        var expected = new Token[]
        {
            new(Number, "1.23", SourcePosition.Empty),
            new(Number, "023.14778", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void IdentifierLexing()
    {
        var source = "foo bar1 b1az";
        var expected = new Token[]
        {
            new(Identifier, "foo", SourcePosition.Empty),
            new(Identifier, "bar1", SourcePosition.Empty),
            new(Identifier, "b1az", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void KeywordsLexing()
    {
        var source = "program FOR iF Then";
        var expected = new Token[]
        {
            new(Program, "program", SourcePosition.Empty),
            new(For, "FOR", SourcePosition.Empty),
            new(If, "iF", SourcePosition.Empty),
            new(Then, "Then", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ComparisonOperatorsLexing()
    {
        var source = "< <= >= ><>==";
        var expected = new Token[]
        {
            new(LessThen, "<", SourcePosition.Empty),
            new(LessThenOrEqual, "<=", SourcePosition.Empty),
            new(GreaterThenOrEqual, ">=", SourcePosition.Empty),
            new(GreaterThen, ">", SourcePosition.Empty),
            new(NotEqual, "<>", SourcePosition.Empty),
            new(Equal, "==", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ArithmeticOperatorsLexing()
    {
        var source = "+-*/";
        var expected = new Token[]
        {
            new(Addition, "+", SourcePosition.Empty),
            new(Subtraction, "-", SourcePosition.Empty),
            new(Multiplication, "*", SourcePosition.Empty),
            new(Division, "/", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void SimpleOperatorsLexing()
    {
        var source = "=(),;:";
        var expected = new Token[]
        {
            new(Assignment, "=", SourcePosition.Empty),
            new(OpenParenthesis, "(", SourcePosition.Empty),
            new(CloseParenthesis, ")", SourcePosition.Empty),
            new(Comma, ",", SourcePosition.Empty),
            new(Semicolon, ";", SourcePosition.Empty),
            new(Colon, ":", SourcePosition.Empty),
            new(EoF, "", SourcePosition.Empty),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void StringsLexing()
    {
        var source = "\"=(),;:\"";
        var expected = new Token[]
        {
            new(TokenType.String, "\"=(),;:\"", new SourcePosition(0, 1, 1, 8)),
            new(EoF, "", new SourcePosition(8, 1, 9, 0)),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result);
    }


    [TestMethod]
    public void CommentsLexing()
    {
        var source = "! =(),;:";
        var expected = new Token[]
        {
            new(Comment, " =(),;:", new SourcePosition(0, 1, 1, 8)),
            new(EoF, "", new SourcePosition(8, 1, 9, 0)),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TwoLineOfCommentsLexing()
    {
        var commentText = " =(),;:";
        var commentLength = commentText.Length + 1;

        var source =
           $"""
           !{commentText}
           !{commentText}
           """;
        var expected = new Token[]
        {
            new(Comment, " =(),;:", new SourcePosition(0, 1, 1, commentLength)),
            new(EoL, Environment.NewLine, new SourcePosition(commentLength, 1, commentLength + 1, _newLineLength)),
            new(Comment, " =(),;:", new SourcePosition(commentLength + _newLineLength, 2, 1, commentLength)),
            new(EoF, "", new SourcePosition(commentLength * 2 + _newLineLength, 2, commentLength + 1, 0)),
        };

        var result = new Lexer(source).Lex().ToArray();

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void MultilineLexing()
    {
        const int lineOneLength = 3;
        const int lineTwoLength = 2;
        const int lineThreeLength = 1;
        const int lineFourLength = 0;

        var source =
           """
           for
           to
           +

           -
           """;
        var expected = new Token[]
        {
            new(EoL, Environment.NewLine, new(lineOneLength, 1, lineOneLength + 1, _newLineLength)),
            new(EoL, Environment.NewLine, new(lineOneLength + lineTwoLength + _newLineLength, 2, lineTwoLength + 1, _newLineLength)),
            new(EoL, Environment.NewLine, new(lineOneLength + lineTwoLength + lineThreeLength + _newLineLength * 2, 3 , lineThreeLength + 1, _newLineLength)),
            new(EoL, Environment.NewLine, new(lineOneLength + lineTwoLength + lineThreeLength + lineFourLength + _newLineLength * 3, 4 , lineFourLength + 1, _newLineLength)),
        };

        var result = new Lexer(source).Lex().Where(t => t.Type == EoL).ToArray();

        CollectionAssert.AreEqual(expected, result);
    }

    private class TokenComparer : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            return x is Token left && y is Token right
                && left.Type == right.Type
                && left.Value == right.Value ? 0 : -1;
        }
    }
}

public static class ExtensionHelpers
{
    extension(SourcePosition source)
    {
        internal static SourcePosition Empty => new(0, 0, 0, 0);
    }
}