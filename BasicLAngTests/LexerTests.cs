using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Tests;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void IntegerLexing()
    {
        var source = "1 23 023 14778";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Number, 0, 0, 0, "1", new WhiteSpaceTrivia(0, 0)),
            new Token(Number, 0, 0, 0, "23", new WhiteSpaceTrivia(0, 0)),
            new Token(Number, 0, 0, 0, "023", new WhiteSpaceTrivia(0, 0)),
            new Token(Number, 0, 0, 0, "14778", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 0, 0, 0, "", new WhiteSpaceTrivia(0, 0)),
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
            new Token(Number, 0, 0, 0, "1.23", new WhiteSpaceTrivia(0, 0)),
            new Token(Number, 0, 0, 0, "023.14778", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 0, 0, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ComparsionOperators()
    {
        var source = "< <= >= ><>==";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(LessThen, 0, 0, 0, "<", new WhiteSpaceTrivia(0, 0)),
            new Token(LessThenOrEqual, 0, 0, 0, "<=", new WhiteSpaceTrivia(0, 0)),
            new Token(GreaterThenOrEqual, 0, 0, 0, ">=", new WhiteSpaceTrivia(0, 0)),
            new Token(GreaterThen, 0, 0, 0, ">", new WhiteSpaceTrivia(0, 0)),
            new Token(NotEqual, 0, 0, 0, "<>", new WhiteSpaceTrivia(0, 0)),
            new Token(Equal, 0, 0, 0, "==", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 0, 0, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void ArithmeticOperators()
    {
        var source = "+-*/";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Addition, 0, 0, 0, "+", new WhiteSpaceTrivia(0, 0)),
            new Token(Subtraction, 0, 0, 0, "-", new WhiteSpaceTrivia(0, 0)),
            new Token(Multiplication, 0, 0, 0, "*", new WhiteSpaceTrivia(0, 0)),
            new Token(Division, 0, 0, 0, "/", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 0, 0, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void SimpleOperators()
    {
        var source = "=(),;:";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Assignment, 0, 0, 0, "=", new WhiteSpaceTrivia(0, 0)),
            new Token(OpenParenthesis, 0, 0, 0, "(", new WhiteSpaceTrivia(0, 0)),
            new Token(CloseParenthesis, 0, 0, 0, ")", new WhiteSpaceTrivia(0, 0)),
            new Token(Comma, 0, 0, 0, ",", new WhiteSpaceTrivia(0, 0)),
            new Token(Semicolon, 0, 0, 0, ";", new WhiteSpaceTrivia(0, 0)),
            new Token(Colon, 0, 0, 0, ":", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 0, 0, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result, new TokenComparer());
    }

    [TestMethod]
    public void Strings()
    {
        var source = "\"=(),;:\"";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(TokenType.String, 1, 1, 8, "=(),;:", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 1, 9, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result);
    }


    [TestMethod]
    public void Comments()
    {
        var source = "! =(),;:";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Comment, 1, 1, 8, " =(),;:", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 1, 9, 0, "", new WhiteSpaceTrivia(0, 0)),
        };

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TwoLineOfComments()
    {
        var source = $"! =(),;:{Environment.NewLine}! =(),;:";
        var result = new Lexer(source).Lex().ToArray();

        var expected = new Token[]
        {
            new Token(Comment, 1, 1, 8, " =(),;:", new WhiteSpaceTrivia(0, 0)),
            new Token(EoL, 1, 9, Environment.NewLine.Length, Environment.NewLine, new WhiteSpaceTrivia(0, 0)),
            new Token(Comment, 2, 1, 8, " =(),;:", new WhiteSpaceTrivia(0, 0)),
            new Token(EoF, 2, 9, 0, "", new WhiteSpaceTrivia(0, 0)),
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