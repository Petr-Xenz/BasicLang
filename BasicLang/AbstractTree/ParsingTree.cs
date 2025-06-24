namespace BasicLang.AbstractTree;

internal class ParseSyntaxTree
{
    public ParseSyntaxTree(IStatement rootStatement, IEnumerable<ProgramError> errors, string sourceCode)
    {
        RootStatement = rootStatement;
        Errors = errors;
        SourceCode = sourceCode;
    }

    public IStatement RootStatement { get; }

    public IEnumerable<ProgramError> Errors { get; }

    public string SourceCode { get; }
}
