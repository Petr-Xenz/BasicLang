using BasicLang.AbstractTree.Statements;
using BasicLang.AbstractTree.Statements.Expressions;

namespace BasicLang.Tests;

[TestClass]
public class ParserTests
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
    public void VariableDeclarationStatementPositiveIntegerLiteral()
    {
        var source = "let foo = 42";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var literalExpression = assignmentExpression.Right as IntegerLiteralExpression;
        Assert.AreEqual(42, literalExpression?.LiteralValue);
    }

    [TestMethod]
    public void VariableDeclarationStatementPositiveFloatLiteral()
    {
        var source = "let foo = 4.2";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var literalExpression = assignmentExpression.Right as FloatLiteralExpression;
        Assert.AreEqual(4.2, literalExpression?.LiteralValue);
    }

    [TestMethod]
    public void IfStatement()
    {
        var source = "If 1 Then print 5";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var ifStatement = tree.RootStatement as IfStatement;
        Assert.IsNotNull(ifStatement);
        var condition = ifStatement.Condition as IntegerLiteralExpression;
        Assert.IsNotNull(condition);

        Assert.AreEqual(1L, condition.LiteralValue);

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

    [TestMethod]
    public void ProgramStatement()
    {
        var source = """
                        program foo
                            goto 10
                        end
                     """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();
        var root = tree.RootStatement as ProgramStatement;
        Assert.IsNotNull(root, "root");

        var gotoStatement = root.Children.Single() as GotoStatement;
        Assert.IsNotNull(gotoStatement, "goto");
        Assert.AreEqual("10", gotoStatement.LineValue);
    }
}