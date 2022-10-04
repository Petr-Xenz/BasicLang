namespace BasicLang.AbstractTree;

internal interface ISyntaxNode : ICodeElement
{
    public IEnumerable<ISyntaxNode> Children { get; }
}