using BasicLang.Compiling;

namespace BasicLang.Runtime;
internal class BasicRuntime
{
    private readonly byte[] _program;
    private int _position;
    private Stack<object> _stack = new();

    private readonly IReadOnlyDictionary<byte, Action<object[]>> _systemCalls = new Dictionary<byte, Action<object[]>>()
    {
        [(byte)SystemCalls.WriteLine] = ConsoleWriteSystemCall,
    };

    public BasicRuntime(byte[] program)
    {
        _program = program;
    }

    public void Execute()
    {
        _position = Compiler.HeaderSize; //skip header;
        while (_position < _program.Length)
        {
            _ = _program[_position] switch
            {
                33 => ExecutePutIntegerOnStack(),
                40 => ExecuteMethodCall(),
                _ => throw new InvalidOperationException($"Unknown opcode: {_program[_position]}"),
            };
        }
    }

    private Unit ExecutePutIntegerOnStack()
    {
        _position++;
        _stack.Push(BitConverter.ToInt64(_program.AsSpan(_position, 8)));
        _position += 8;
        return Unit.Instance;
    }

    private Unit ExecuteMethodCall()
    {
        _position++;
        var callAddress = _program[_position];
        if (_systemCalls.TryGetValue(callAddress, out var call))
        {
            _position++;
            var argsCount = _program[_position];
            _position++;
            var args = new object[argsCount];
            for (var i = 0; i < argsCount; i++)
            {
                args[i] = _stack.Pop();
            }

            call(args);
        }
        else
        {

        }
        return Unit.Instance;
    }

    private static void ConsoleWriteSystemCall(object[] args)
    {
        Console.WriteLine(args.Select((o, i) => $"{{{i}}}").Aggregate((p, c) => $"{p}{c}"), args);
    }

    private class Unit
    {
        private Unit() { }
        public static Unit Instance { get; } = new();
    }
}
