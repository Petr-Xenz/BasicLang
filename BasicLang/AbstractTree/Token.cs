namespace BasicLang.AbstractTree;

internal record Token : ICodeElement
{
    public Token(TokenType type, string value, SourcePosition position)
    {
        Type = type;
        Value = value;
        SourcePosition = position;
    }

    public TokenType Type { get; }

    public string Value { get; }

    public SourcePosition SourcePosition { get; }
}
