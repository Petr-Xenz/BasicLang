namespace BasicLang.Tests.ParserTests;

[TestClass]
public class LoopTests
{
    [TestMethod]
    public void ForStatement()
    {
        var source =
                """
                for
                    x = 5
                    y = 6
                next
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var forStatement = tree.RootStatement as ForStatement;
        Assert.IsNotNull(forStatement);

        Assert.IsNull(forStatement.Counter);
        Assert.AreEqual(2, forStatement.InnerStatements.Count());
    }

    [TestMethod]
    public void ForStatementWithSimpleCounter()
    {
        var source =
                """
                for i = 1 to 5
                    x = 5
                    y = 6
                next
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var forStatement = tree.RootStatement as ForStatement;
        Assert.IsNotNull(forStatement);

        var forCounter = forStatement.Counter;
        Assert.IsNotNull(forCounter);
        Assert.IsInstanceOfType(forCounter.CounterVariable, typeof(AssignmentExpression));
        Assert.AreEqual("5", forCounter.Limit.Value);
        Assert.AreEqual(1L, forCounter.Step);

        Assert.AreEqual(3, forStatement.Children.Count());
    }

    [TestMethod]
    public void ForStatementWithStepCounter()
    {
        var source =
                """
                for i = 1 to 6 step 2
                    x = 5
                    y = 6
                next
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var forStatement = tree.RootStatement as ForStatement;
        Assert.IsNotNull(forStatement);

        var forCounter = forStatement.Counter;
        Assert.IsNotNull(forCounter);
        Assert.IsInstanceOfType(forCounter.CounterVariable, typeof(AssignmentExpression));
        Assert.AreEqual("6", forCounter.Limit.Value);
        Assert.AreEqual(2L, forCounter.Step);

        Assert.AreEqual(3, forStatement.Children.Count());
    }

    [TestMethod]
    public void WhileStatement()
    {
        var source =
                """
                while true
                    x = 5
                    y = 6
                loop
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var whileStatement = tree.RootStatement as WhileStatement;
        Assert.IsNotNull(whileStatement);

        var condition = whileStatement.Condition as BooleanExpression;
        Assert.AreEqual(true, condition?.LiteralValue);
        Assert.AreEqual(2, whileStatement.InnerStatements.Count());
    }

    [TestMethod]
    public void DoUntilStatement()
    {
        var source =
                """
                do
                    x = 5
                    y = 6
                until true
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var doUntilStatement = tree.RootStatement as DoUntilStatement;
        Assert.IsNotNull(doUntilStatement);

        var condition = doUntilStatement.Condition as BooleanExpression;
        Assert.AreEqual(true, condition?.LiteralValue);
        Assert.AreEqual(2, doUntilStatement.InnerStatements.Count());
    }
}