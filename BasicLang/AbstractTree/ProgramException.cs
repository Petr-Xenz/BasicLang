using System.Runtime.Serialization;

namespace BasicLang.AbstractTree;

internal class ProgramException : Exception
{

    public SourcePosition Position { get; }
    public ProgramException(string? message, SourcePosition position) : base(message)
    {
        Position = position;
    }

    protected ProgramException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}