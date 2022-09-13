namespace BasicLang.AbstractTree
{
    internal interface IExpression : IStatement
    {
        new IEnumerable<IExpression> Children { get; }
    }
}