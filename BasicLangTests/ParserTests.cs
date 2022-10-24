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
    public void VariableAssignmentStatementWithOmittedLetKeyword()
    {
        var source = "foo = 42";
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

        var whileStatement = tree.RootStatement as DoUntilStatement;
        Assert.IsNotNull(whileStatement);

        var condition = whileStatement.Condition as BooleanExpression;
        Assert.AreEqual(true, condition?.LiteralValue);
        Assert.AreEqual(2, whileStatement.InnerStatements.Count());
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