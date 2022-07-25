namespace BasicLang.AbstractTree
{
    public interface ISourceLocation
    {
        int Line { get; }

        int Column { get; }

        int Length { get; }

        string Value { get; }

        WhiteSpaceTrivia WhiteSpaceTrivia { get; }
    }
}
