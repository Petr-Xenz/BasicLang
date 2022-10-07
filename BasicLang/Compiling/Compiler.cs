﻿using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements;
using BasicLang.AbstractTree.Statements.Expressions;
using System.Buffers;
using static System.Reflection.Emit.OpCodes;

namespace BasicLang.Compiling;
internal class Compiler
{
    private readonly ParseSyntaxTree _tree;
    private readonly Dictionary<string, int> _labelsLocations = new();
    private readonly Dictionary<int, int> _sourceToOpCodeLocation = new();
    private readonly List<byte> _constants = new();

    private int _stackDepth;

    public Compiler(ParseSyntaxTree tree)
    {
        _tree = tree;
    }

    public byte[] Compile()
    {
        var header = new List<byte>(32);
        var instructions = CompileNode(_tree.RootStatement);
        return header.Concat(instructions).Concat(_constants).ToArray();
    }

    private IEnumerable<byte> CompileNode(IStatement statement)
    {
        return statement switch
        {
            PrintStatement print => CompilePrintStatement(print),
            IntegerLiteralExpression integer => CompileIntegerLiteral(integer),
            _ => throw new NotSupportedException(statement.GetType().Name),
        };
    }

    private IEnumerable<byte> CompileIntegerLiteral(IntegerLiteralExpression integer)
    {
        _stackDepth += 1;
        return BitConverter.GetBytes(integer.LiteralValue)
            .Prepend((byte)Ldc_I8.Value);
    }

    private IEnumerable<byte> CompilePrintStatement(PrintStatement print)
    {
        var result = print.Expressions
            .Select(CompileNode)
            .SelectMany(op => op)
            .ToArray()
            .Concat(new byte[] { (byte)Call.Value, (byte)SystemCalls.WriteLine, (byte)_stackDepth });

        _stackDepth = 0;
        return result;
    }
}

internal enum SystemCalls : byte
{
    Unknown,
    WriteLine,
}
