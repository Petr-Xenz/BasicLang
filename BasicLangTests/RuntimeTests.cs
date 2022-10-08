using BasicLang.Compiling;
using BasicLang.Runtime;

namespace BasicLang.Tests;

[TestClass]
public class RuntimeTests
{
    private readonly TextWriter _consoleWriter = Console.Out;


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

        var writer = new StringWriter();
        Console.SetOut(writer);

        new BasicRuntime(opcodes).Execute();

        Assert.AreEqual("5\r\n", writer.ToString());
    }

    [TestMethod]
    public void PrintAdditionOfIntegerLiterals()
    {
        var source = "print 5 + 5";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        var writer = new StringWriter();
        Console.SetOut(writer);

        new BasicRuntime(opcodes).Execute();

        Assert.AreEqual("10\r\n", writer.ToString());
    }
}