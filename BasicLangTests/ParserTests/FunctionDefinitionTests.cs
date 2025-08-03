namespace BasicLang.Tests.ParserTests;

[TestClass]
public class FunctionDefinitionTests
{
    [TestMethod]
    public void FunctionStatementSingleArgument()
    {
        var source =
                """
                def foo(bar)
                    x = bar
                    y = 6
                fnend
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var functionStatement = tree.RootStatement as FunctionStatement;
        Assert.IsNotNull(functionStatement);

        Assert.AreEqual(1, functionStatement.Arguments.Count());
        Assert.AreEqual(2, functionStatement.InnerStatements.Count());
    }

    [TestMethod]
    public void FunctionStatementMultipleArguments()
    {
        var source =
                """
                def foo(bar, baz)
                    x = bar
                    y = baz
                fnend
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var functionStatement = tree.RootStatement as FunctionStatement;
        Assert.IsNotNull(functionStatement);

        Assert.AreEqual(2, functionStatement.Arguments.Count());
        Assert.AreEqual(2, functionStatement.InnerStatements.Count());
    }


    [TestMethod]
    public void FunctionStatementNoArguments()
    {
        var source =
                """
                def foo()
                    x = bar
                    y = baz
                fnend
                """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var functionStatement = tree.RootStatement as FunctionStatement;
        Assert.IsNotNull(functionStatement);

        Assert.AreEqual(0, functionStatement.Arguments.Count());
        Assert.AreEqual(2, functionStatement.InnerStatements.Count());
    }
}