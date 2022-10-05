using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Tests;

[TestClass]
public partial class LexerTests
{
    [TestMethod]
    public void IntegerLexing()
    {
        var source = "1 23 023 14778";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Number, "1", new SourcePosition(0, 0, 0, 0)),
        new Token(Number, "23", new SourcePosition(0, 0, 0, 0)),
        new Token(Number, "023", new SourcePosition(0, 0, 0, 0)),
        new Token(Number, "14778", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void FloatLexing()
    {
        var source = "1.23 023.14778";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Number, "1.23", new SourcePosition(0, 0, 0, 0)),
        new Token(Number, "023.14778", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void IdentifierLexing()
    {
        var source = "foo bar1 b1az";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Identifier, "foo", new SourcePosition(0, 0, 0, 0)),
        new Token(Identifier, "bar1", new SourcePosition(0, 0, 0, 0)),
        new Token(Identifier, "b1az", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void KeywordsLexing()
    {
        var source = "program FOR iF Then";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Program, "program", new SourcePosition(0, 0, 0, 0)),
        new Token(For, "FOR", new SourcePosition(0, 0, 0, 0)),
        new Token(If, "iF", new SourcePosition(0, 0, 0, 0)),
        new Token(Then, "Then", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ComparsionOperatorsLexing()
    {
        var source = "< <= >= ><>==";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(LessThen, "<", new SourcePosition(0, 0, 0, 0)),
        new Token(LessThenOrEqual, "<=", new SourcePosition(0, 0, 0, 0)),
        new Token(GreaterThenOrEqual, ">=", new SourcePosition(0, 0, 0, 0)),
        new Token(GreaterThen, ">", new SourcePosition(0, 0, 0, 0)),
        new Token(NotEqual, "<>", new SourcePosition(0, 0, 0, 0)),
        new Token(Equal, "==", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ArithmeticOperatorsLexing()
    {
        var source = "+-*/";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Addition, "+", new SourcePosition(0, 0, 0, 0)),
        new Token(Subtraction, "-", new SourcePosition(0, 0, 0, 0)),
        new Token(Multiplication, "*", new SourcePosition(0, 0, 0, 0)),
        new Token(Division, "/", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void SimpleOperatorsLexing()
    {
        var source = "=(),;:";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Assignment, "=", new SourcePosition(0, 0, 0, 0)),
        new Token(OpenParenthesis, "(", new SourcePosition(0, 0, 0, 0)),
        new Token(CloseParenthesis, ")", new SourcePosition(0, 0, 0, 0)),
        new Token(Comma, ",", new SourcePosition(0, 0, 0, 0)),
        new Token(Semicolon, ";", new SourcePosition(0, 0, 0, 0)),
        new Token(Colon, ":", new SourcePosition(0, 0, 0, 0)),
        new Token(EoF, "", new SourcePosition(0, 0, 0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void StringsLexing()
    {
        var source = "\"=(),;:\"";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(TokenType.String, "\"=(),;:\"", new SourcePosition(0, 1, 1, 8)),
            new Token(EoF, "", new SourcePosition(8, 1, 9, 0)),
        };

        CollectionAssert.AreEqual(expected, result);
    }


    [TestMethod]
    public void CommentsLexing()
    {
        var source = "! =(),;:";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
        new Token(Comment, " =(),;:", new SourcePosition(0, 1, 1, 8)),
        new Token(EoF, "", new SourcePosition(8, 1, 9, 0)),
        };

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TwoLineOfCommentsLexing()
    {
        var source = 
           """
           ! =(),;:
           ! =(),;:
           """;
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Comment, " =(),;:", new SourcePosition(0,1, 1, 8)),
            new Token(EoL, Environment.NewLine, new SourcePosition(8, 1, 9, Environment.NewLine.Length)),
            new Token(Comment, " =(),;:", new SourcePosition(10, 2, 1, 8)),
            new Token(EoF, "", new SourcePosition(18, 2, 9, 0)),
        };

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