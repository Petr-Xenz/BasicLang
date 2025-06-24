namespace BasicLang.AbstractTree;

internal interface ISyntaxNode : ICodeElement
{
    IEnumerable<ISyntaxNode> Children { get; }
}