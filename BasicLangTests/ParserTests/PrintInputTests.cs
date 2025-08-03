namespace BasicLang.Tests.ParserTests;

[TestClass]
public class PrintInputTests
{
    [TestMethod]
    public void PrintStatementSingleExpression()
    {
        var source = "print foo";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var printStatement = tree.RootStatement as PrintStatement;
        Assert.IsNotNull(printStatement);
        Assert.AreEqual("foo", (printStatement.Expressions.Single() as VariableExpression)?.Value);
    }

    [TestMethod]
    public void PrintStatementMultipleExpressions()
    {
        var source = "print foo, 1; bar + 2";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var printStatement = tree.RootStatement as PrintStatement;
        Assert.IsNotNull(printStatement);
        var expressions = printStatement.Expressions.ToList();

        Assert.AreEqual("foo", (expressions[0] as VariableExpression)?.Value);
        Assert.AreEqual("1", (expressions[1] as IntegerLiteralExpression)?.Value);
        Assert.AreEqual("bar + 2", (expressions[2] as AdditionExpression)?.Value);
    }

    [TestMethod]
    public void InputStatementSingleExpression()
    {
        var source = "input foo";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);
        Assert.AreEqual("foo", (inputStatement.Expressions.Single() as VariableExpression)?.Value);
    }

    [TestMethod]
    public void InputStatementMultipleExpressions()
    {
        var source = "input foo, bar; baz";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);
        var expressions = inputStatement.Expressions.ToList();

        Assert.AreEqual("foo", (expressions[0] as VariableExpression)?.Value);
        Assert.AreEqual("bar", (expressions[1] as VariableExpression)?.Value);
        Assert.AreEqual("baz", (expressions[2] as VariableExpression)?.Value);
    }
}