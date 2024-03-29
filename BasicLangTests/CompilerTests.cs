﻿using BasicLang.Compiling;
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

        var expected = new byte[Compiler.HeaderSize]
            .Concat(new byte[]
            {
                (byte)Ldc_I8.Value,
                5, 0, 0, 0, 0, 0, 0, 0,
                (byte)Call.Value,
                (byte)SystemCalls.WriteLine,
                1,
            })
            .ToArray();

        CollectionAssert.AreEquivalent(expected, opcodes);
    }

    [TestMethod]
    public void CompilingPrintMultipleIntegerLiterals()
    {
        var source = "print 5, 6";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        var expected = new byte[Compiler.HeaderSize]
            .Concat(new byte[]
            {
                (byte)Ldc_I8.Value,
                6, 0, 0, 0, 0, 0, 0, 0,
                (byte)Ldc_I8.Value,
                5, 0, 0, 0, 0, 0, 0, 0,
                (byte)Call.Value,
                (byte)SystemCalls.WriteLine,
                2,
            })
            .ToArray();

        CollectionAssert.AreEquivalent(expected, opcodes);
    }

    [TestMethod]
    public void CompilingPrintAdditionOfIntegerLiterals()
    {
        var source = "print 5 + 5";
        var tokens = new Lexer(source).Lex();
        var tree = new Parser(tokens, source).Parse();
        var opcodes = new Compiler(tree).Compile();

        var expected = new byte[Compiler.HeaderSize]
            .Concat(new byte[]
            {
                (byte)Ldc_I8.Value,
                5, 0, 0, 0, 0, 0, 0, 0,
                (byte)Ldc_I8.Value,
                5, 0, 0, 0, 0, 0, 0, 0,
                (byte)Add.Value,
                (byte)Call.Value,
                (byte)SystemCalls.WriteLine,
                1,
            })
            .ToArray();

        CollectionAssert.AreEquivalent(expected, opcodes);
    }
}