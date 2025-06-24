using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements;
using BasicLang.AbstractTree.Statements.Expressions;
using System.Buffers;
using static System.Reflection.Emit.OpCodes;

namespace BasicLang.Compiling;

internal class Compiler
{
    public const int HeaderSize = 32;

    private readonly ParseSyntaxTree _tree;
    private readonly Dictionary<string, int> _labelsLocations = [];
    private readonly Dictionary<int, int> _sourceToOpCodeLocation = [];
    private readonly List<byte> _constants = [];

    private int _stackDepth;

    public Compiler(ParseSyntaxTree tree)
    {
        _tree = tree;
    }

    public byte[] Compile()
    {
        var header = new byte[HeaderSize];
        var instructions = CompileNode(_tree.RootStatement);
        return header.Concat(instructions).Concat(_constants).ToArray();
    }

    private IEnumerable<byte> CompileNode(IStatement statement)
    {
        return statement switch
        {
            PrintStatement print => CompilePrintStatement(print),
            IntegerLiteralExpression integer => CompileIntegerLiteral(integer),
            AdditionExpression addition => CompileAdd(addition),
            _ => throw new NotSupportedException(statement.GetType().Name),
        };
    }

    private IEnumerable<byte> CompileAdd(AdditionExpression addition)
    {
        var left = CompileNode(addition.Left);
        var right = CompileNode(addition.Right);
        _stackDepth -= 1;

        return left.Concat(right).Append((byte)Add.Value);
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
            .Reverse()
            .Select(CompileNode)
            .SelectMany(op => op)
            .ToArray()
            .Concat([(byte)Call.Value, (byte)SystemCalls.WriteLine, (byte)_stackDepth]);

        _stackDepth = 0;
        return result;
    }
}

internal enum SystemCalls : byte
{
    Unknown,
    WriteLine,
}
