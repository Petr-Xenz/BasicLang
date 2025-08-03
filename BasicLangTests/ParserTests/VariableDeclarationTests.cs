namespace BasicLang.Tests.ParserTests;

[TestClass]
public class VariableDeclarationTests
{
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
}