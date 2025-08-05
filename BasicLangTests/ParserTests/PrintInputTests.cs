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

        var expression = Assert.ContainsSingle(inputStatement.InputExpressions);
        Assert.AreEqual("foo", expression.Value);
        Assert.IsNull(expression.PromptExpression);
    }

    [TestMethod]
    public void InputStatementMultipleExpressions()
    {
        var source = "input foo, bar; baz";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);

        var expression = Assert.ContainsSingle(inputStatement.InputExpressions);
        Assert.IsNull(expression.PromptExpression);
        var variableExpressions = expression.Variables.ToArray();

        Assert.AreEqual("foo", variableExpressions[0].Value);
        Assert.AreEqual("bar", variableExpressions[1].Value);
        Assert.AreEqual("baz", variableExpressions[2].Value);
    }

    [TestMethod]
    public void InputStatementPromptExpression()
    {
        var source = """input "Prompt" foo""";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);
        var expression = Assert.ContainsSingle(inputStatement.InputExpressions);

        Assert.IsNotNull(expression.PromptExpression);
        Assert.AreEqual("\"Prompt\"", expression.PromptExpression.Value);

        var variableExpression = Assert.ContainsSingle(expression.Variables);
        Assert.AreEqual("foo", variableExpression.Value);
    }

    [TestMethod]
    public void InputStatementPromptExpressionMultipleInputs()
    {
        var source = """input "Prompt" foo, bar""";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);
        var expression = Assert.ContainsSingle(inputStatement.InputExpressions);

        Assert.IsNotNull(expression.PromptExpression);
        Assert.AreEqual("\"Prompt\"", expression.PromptExpression.Value);

        var variableExpressions = expression.Variables.ToArray();
        Assert.HasCount(2, variableExpressions);
        Assert.AreEqual("foo", variableExpressions[0].Value);
        Assert.AreEqual("bar", variableExpressions[1].Value);
    }

    [TestMethod]
    public void InputStatementMultiplePromptExpressionWithIndividualInputs()
    {
        var source = """input "Prompt" foo "Second text" bar""";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var inputStatement = tree.RootStatement as InputStatement;
        Assert.IsNotNull(inputStatement);
        var expressions = inputStatement.InputExpressions.ToArray();
        Assert.HasCount(2, expressions);

        var expression1 = expressions[0];
        Assert.IsNotNull(expression1.PromptExpression);
        Assert.AreEqual("\"Prompt\"", expression1.PromptExpression.Value);

        var variableExpression1 = Assert.ContainsSingle(expression1.Variables);
        Assert.AreEqual("foo", variableExpression1.Value);

        var expression2 = expressions[1];
        Assert.IsNotNull(expression2.PromptExpression);
        Assert.AreEqual("\"Second text\"", expression2.PromptExpression.Value);

        var variableExpression2 = Assert.ContainsSingle(expression2.Variables);
        Assert.AreEqual("bar", variableExpression2.Value);
    }
}