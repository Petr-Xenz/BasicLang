namespace BasicLang.AbstractTree;

internal readonly struct SourcePosition
{
    public SourcePosition(int offset, int line, int column, int length, WhiteSpaceTrivia whiteSpaceTrivia = default)
    {
        Offset = offset;
        Line = line;
        Column = column;
        Length = length;
        WhiteSpaceTrivia = whiteSpaceTrivia;
    }

    public int Offset { get; }

    public int Line { get; }

    public int Column { get; }

    public int Length { get; }

    private WhiteSpaceTrivia WhiteSpaceTrivia { get; }

    public override string ToString()
    {
        return $"{Line}:{Column}";
    }

}
