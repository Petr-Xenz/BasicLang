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