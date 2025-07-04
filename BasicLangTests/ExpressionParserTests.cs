﻿using BasicLang.AbstractTree.Statements.Expressions;
using BasicLang.AbstractTree.Statements;

namespace BasicLang.Tests;

[TestClass]
public class ExpressionParserTests
{

    [TestMethod]
    public void IntegerAddition() =>
        SimpleBinaryExpressionTemplate<AdditionExpression, NumberExpression<long>, long>("let foo = 4 + 1", "foo", 4L, 1L);


    [TestMethod]
    public void FloatAddition() =>
        SimpleBinaryExpressionTemplate<AdditionExpression, NumberExpression<double>, double>("let foo = 3.14 + 2.8", "foo", 3.14, 2.8);

    [TestMethod]
    public void IntegerSubtraction() =>
        SimpleBinaryExpressionTemplate<SubtractionExpression, NumberExpression<long>, long>("let foo = 4 - 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerDivision() =>
        SimpleBinaryExpressionTemplate<DivisionExpression, NumberExpression<long>, long>("let foo = 4 / 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerMultiplication() =>
            SimpleBinaryExpressionTemplate<MultiplicationExpression, NumberExpression<long>, long>("let foo = 4 * 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerEquality() =>
        SimpleBinaryExpressionTemplate<EqualityExpressions, NumberExpression<long>, long>("let foo = 4 == 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerNonEquality() =>
        SimpleBinaryExpressionTemplate<NonEqualityExpressions, NumberExpression<long>, long>("let foo = 4 <> 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerLessThanExpression() =>
        SimpleBinaryExpressionTemplate<LessThanExpressions, NumberExpression<long>, long>("let foo = 4 < 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerLessThanOrEqualExpression() =>
        SimpleBinaryExpressionTemplate<LessThanOrEqualExpressions, NumberExpression<long>, long>("let foo = 4 <= 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerGreaterThanExpression() =>
        SimpleBinaryExpressionTemplate<GreaterThanExpressions, NumberExpression<long>, long>("let foo = 4 > 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerGreaterThanOrEqualExpression() =>
        SimpleBinaryExpressionTemplate<GreaterThanOrEqualExpressions, NumberExpression<long>, long>("let foo = 4 >= 1", "foo", 4L, 1L);

    [TestMethod]
    public void IntegerOrExpression() =>
        SimpleBinaryExpressionTemplate<OrExpression, NumberExpression<long>, long>("let foo = 4 or 1", "foo", 4L, 1L);


    [TestMethod]
    public void IntegerXorExpression() =>
        SimpleBinaryExpressionTemplate<XorExpression, NumberExpression<long>, long>("let foo = 4 xor 1", "foo", 4L, 1L);


    [TestMethod]
    public void IntegerAndExpression() =>
        SimpleBinaryExpressionTemplate<AndExpression, NumberExpression<long>, long>("let foo = 4 and 1", "foo", 4L, 1L);

    [TestMethod]
    public void MathExpressionsOrder()
    {
        var source = "let foo = 42 + 1 * 3";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var binaryExpression = assignmentExpression.Right as AdditionExpression;
        Assert.IsNotNull(binaryExpression);

        Assert.IsInstanceOfType(binaryExpression.Left, typeof(IntegerLiteralExpression));

        var rightExpression = binaryExpression.Right as MultiplicationExpression;
        Assert.AreEqual("1", rightExpression?.Left.Value);
        Assert.AreEqual("3", rightExpression?.Right.Value);
    }

    [TestMethod]
    public void NotExpression()
    {
        var source = "let foo = not 1";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var notExpression = assignmentExpression.Right as NotExpression;
        Assert.IsNotNull(notExpression);

        var innerExpression = notExpression.Inner as IntegerLiteralExpression;
        Assert.AreEqual("1", innerExpression?.Value);
    }

    [TestMethod]
    public void FunctionCallExpressionZeroParameters()
    {
        var source = "let foo = Bar()";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var functionCallExpression = assignmentExpression.Right as FunctionCallExpression;
        Assert.IsNotNull(functionCallExpression);

        Assert.AreEqual("Bar", functionCallExpression.Name);
        Assert.AreEqual(0, functionCallExpression.Parameters.Count());
    }

    [TestMethod]
    public void FunctionCallComplexExpression()
    {
        var source = "let foo = Bar(1 + 1) + 1";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var additionExpression = assignmentExpression.Right as AdditionExpression;
        Assert.IsNotNull(additionExpression);

        var function = additionExpression.Left as FunctionCallExpression;

        Assert.AreEqual("Bar", function?.Name);
        Assert.AreEqual(1, function?.Parameters.Count());

        var literal = additionExpression.Right as IntegerLiteralExpression;
        Assert.AreEqual(1L, literal?.LiteralValue);

    }

    [TestMethod]
    public void FunctionCallExpressionSingleParameter()
    {
        var source = "let foo = Bar(1)";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var functionCallExpression = assignmentExpression.Right as FunctionCallExpression;
        Assert.IsNotNull(functionCallExpression);

        Assert.AreEqual("Bar", functionCallExpression.Name);
        Assert.AreEqual(1, functionCallExpression.Parameters.Count());
    }

    [TestMethod]
    public void FunctionCallExpressionMultipleParameters()
    {
        var source = "let foo = Bar(1, 2)";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var functionCallExpression = assignmentExpression.Right as FunctionCallExpression;
        Assert.IsNotNull(functionCallExpression);

        Assert.AreEqual("Bar", functionCallExpression.Name);
        Assert.AreEqual(2, functionCallExpression.Parameters.Count());
    }

    [TestMethod]
    public void GroupingExpression()
    {
        var source = "let foo = (1 + 1)";
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var groupingExpression = assignmentExpression.Right as GroupingExpression;
        Assert.IsNotNull(groupingExpression);

        var innerExpression = groupingExpression.Inner as AdditionExpression;
        Assert.IsNotNull(innerExpression);

        Assert.AreEqual("1", innerExpression?.Left.Value);
        Assert.AreEqual("1", innerExpression?.Right.Value);
    }

    [TestMethod]
    public void StringExpression()
    {
        var source = """let foo = "foo" """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual("foo", variableExpression?.Name);

        var stringExpression = assignmentExpression.Right as StringExpression;
        Assert.IsNotNull(stringExpression);

        Assert.AreEqual("foo", stringExpression.LiteralValue);
    }

    private void SimpleBinaryExpressionTemplate<T, U, N>(string source, string varName, object left, object right)
        where T : BinaryExpression
        where U : NumberExpression<N>
        where N : struct
    {
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();

        var variableDeclarationStatement = tree.RootStatement as VariableDeclarationStatement;
        Assert.IsNotNull(variableDeclarationStatement, nameof(variableDeclarationStatement));

        var assignmentExpression = variableDeclarationStatement.Expression.Child as AssignmentExpression;
        Assert.IsNotNull(assignmentExpression, nameof(assignmentExpression));

        var variableExpression = assignmentExpression.Left as VariableExpression;
        Assert.AreEqual(varName, variableExpression?.Name);

        var binaryExpression = assignmentExpression.Right as T;
        Assert.IsNotNull(binaryExpression);

        var leftExpression = binaryExpression.Left as U;
        Assert.AreEqual(left, leftExpression?.LiteralValue);

        var rightExpression = binaryExpression.Right as U;
        Assert.AreEqual(right, rightExpression?.LiteralValue);
    }
}