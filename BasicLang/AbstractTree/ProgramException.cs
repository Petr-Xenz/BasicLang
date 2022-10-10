using System.Runtime.Serialization;

namespace BasicLang.AbstractTree;

internal class ProgramException : Exception
{

    public SourcePosition Position { get; }
    public ProgramException(string? message, SourcePosition position) : base(message)
    {
        Position = position;
    }

    public ProgramException(string? message, ICodeElement element) : base(message)
    {
        Position = element.SourcePosition;
    }

    protected ProgramException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}