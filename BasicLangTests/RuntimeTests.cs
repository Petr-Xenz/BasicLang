using BasicLang.Compiling;
using BasicLang.Runtime;

namespace BasicLang.Tests;

[TestClass]
public class RuntimeTests
{
    private readonly TextWriter _consoleWriter = Console.Out;

    private readonly StringWriter _testWriter = new();

    [TestInitialize]
    public void TestSetup()
    {
        Console.SetOut(_testWriter);
    }

    [TestCleanup]
    public void TestCleanUp()
    {
        Console.SetOut(_consoleWriter);
    }


    [TestMethod]
    public void PrintIntegerLiteral()
    {
        var source = "print 5";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        new BasicRuntime(opcodes).Execute();

        Assert.AreEqual("5", _testWriter.ToString().TrimEnd());
    }

    [TestMethod]
    public void PrintMultipleIntegersLiteral()
    {
        var source = "print 5, 6";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        new BasicRuntime(opcodes).Execute();

        Assert.AreEqual("56", _testWriter.ToString().TrimEnd());
    }

    [TestMethod]
    public void PrintAdditionOfIntegerLiterals()
    {
        var source = "print 5 + 5";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        new BasicRuntime(opcodes).Execute();

        Assert.AreEqual("10", _testWriter.ToString().TrimEnd());
    }
}