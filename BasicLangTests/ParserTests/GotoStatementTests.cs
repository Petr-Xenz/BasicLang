namespace BasicLang.Tests.ParserTests;

[TestClass]
public class GotoStatementTests
{
    [TestMethod]
    public void GotoStatement()
    {
        var source = "goto 10";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var gotoStatement = tree.RootStatement as GotoStatement;
        Assert.IsNotNull(gotoStatement);
        Assert.AreEqual("10", gotoStatement.LineValue);
    }

    [TestMethod]
    public void GotoNamedLabelStatement()
    {
        var source = "goto label";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var gotoStatement = tree.RootStatement as GotoStatement;
        Assert.IsNotNull(gotoStatement);
        Assert.AreEqual("label", gotoStatement.LineValue);
    }

    [TestMethod]
    public void LabelStatement()
    {
        var source = "label:";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var labelStatement = tree.RootStatement as LabelStatement;
        Assert.IsNotNull(labelStatement);
        Assert.AreEqual("label", labelStatement.LineValue);
    }
}