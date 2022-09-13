namespace BasicLang.AbstractTree
{
    internal interface IStatement : ICodeElement
    {
        SourcePosition GeneralErrorPosition { get; }

        string Value { get; }

        IEnumerable<IStatement> Children { get; }
    }
}