namespace BasicLang.AbstractTree;

internal class ProgramError
{
    public ProgramError(string text, SourcePosition position)
    {
        Text = text;
        Position = position;
    }

    public string Text { get; }

    public SourcePosition Position { get; }

}