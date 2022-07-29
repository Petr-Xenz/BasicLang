namespace BasicLang.AbstractTree
{
    internal class ProgramError
    {
        public ProgramError(string text, SourcePosition position)
        {
            Text = text;
            Position = position;
        }

        string Text { get; }

        SourcePosition Position { get; }

    }
}