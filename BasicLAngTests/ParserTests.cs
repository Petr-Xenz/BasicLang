using BasicLang.AbstractTree.Statements;
using BasicLang.AbstractTree.Statements.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        Assert.IsNotNull(root);
        
        var gotoStatement = root.Children.Single() as GotoStatement;
        Assert.IsNotNull(gotoStatement);
        Assert.AreEqual("10", gotoStatement.LineValue);
    }
}