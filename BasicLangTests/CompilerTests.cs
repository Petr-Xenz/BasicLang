using BasicLang.Compiling;
using static System.Reflection.Emit.OpCodes;

namespace BasicLang.Tests;

[TestClass]
public class CompilerTests
{
    [TestMethod]
    public void CompilingPrintIntegerLiteral()
    {
        var source = "print 5";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        var expected = new byte[]
        {
            (byte)Ldc_I8.Value,
            5, 0, 0, 0, 0, 0, 0, 0,
            (byte)Call.Value,
            (byte)SystemCalls.WriteLine,
            1,
        };

        CollectionAssert.AreEquivalent(expected, opcodes);
    }
}