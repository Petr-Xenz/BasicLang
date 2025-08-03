namespace BasicLang.Tests.ParserTests;

[TestClass]
public class ConditionTests
{
        [TestMethod]
    public void IfStatement()
    {
        var source = "If true Then print 5";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as BooleanExpression;
        Assert.IsNotNull(condition);

        Assert.AreEqual(true, condition.LiteralValue);

        var printStatement = ifStatement.OnTrue as PrintStatement;
        Assert.IsNotNull(printStatement);
        Assert.AreEqual("5", (printStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);
    }

    [TestMethod]
    public void IfElseStatement()
    {
        var source = "If 1 Then print 5 else print 6";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as IntegerLiteralExpression;
        Assert.IsNotNull(condition);

        Assert.AreEqual(1L, condition.LiteralValue);

        var onTrueStatement = ifStatement.OnTrue as PrintStatement;
        Assert.IsNotNull(onTrueStatement);
        Assert.AreEqual("5", (onTrueStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);

        var onFalseStatement = ifStatement.OnFalse as PrintStatement;
        Assert.IsNotNull(onFalseStatement);
        Assert.AreEqual("6", (onFalseStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);
    }

    [TestMethod]
    public void IfElseStatement2()
    {
        var source = "If 1 Then goto foo else print 6";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as IntegerLiteralExpression;
        Assert.IsNotNull(condition);

        Assert.AreEqual(1L, condition.LiteralValue);

        var onTrueStatement = ifStatement.OnTrue as GotoStatement;
        Assert.IsNotNull(onTrueStatement);
        Assert.AreEqual("foo", onTrueStatement.LineValue);

        var onFalseStatement = ifStatement.OnFalse as PrintStatement;
        Assert.IsNotNull(onFalseStatement);
        Assert.AreEqual("6", (onFalseStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);
    }

    [TestMethod]
    public void ElseIfStatement()
    {
        var source = "If 1 Then goto foo elseif 2 then print 6 elseif 3 then print 6";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as IntegerLiteralExpression;
        Assert.IsNotNull(condition);

        Assert.AreEqual(1L, condition.LiteralValue);

        var onTrueStatement = ifStatement.OnTrue as GotoStatement;
        Assert.IsNotNull(onTrueStatement);
        Assert.AreEqual("foo", onTrueStatement.LineValue);

        Assert.AreEqual(2, ifStatement.ElseIfStatements.Count());
        foreach (var s in ifStatement.ElseIfStatements)
        {
            var c = s.Condition as IntegerLiteralExpression;
            Assert.IsNotNull(c);
            var elseIfStatement = s.OnTrue as PrintStatement;
            Assert.IsNotNull(elseIfStatement);
            Assert.AreEqual("6", (elseIfStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);
        }
    }

    [TestMethod]
    public void IfStatementComplexCondition()
    {
        var source = "If 1 + 1 Then print 5";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as AdditionExpression;
        Assert.IsNotNull(condition);

        var printStatement = ifStatement.OnTrue as PrintStatement;
        Assert.IsNotNull(printStatement);
        Assert.AreEqual("5", (printStatement.Expressions.Single() as IntegerLiteralExpression)?.Value);
    }
}