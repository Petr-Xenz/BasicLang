namespace BasicLang.AbstractTree;

internal readonly struct WhiteSpaceTrivia
{
    public WhiteSpaceTrivia(int leadingSpacesCount, int trailingSpacesCount)
    {
        LeadingSpacesCount = leadingSpacesCount;
        TrailingSpacesCount = trailingSpacesCount;
    }

    public int LeadingSpacesCount { get; }
    public int TrailingSpacesCount { get; }
}