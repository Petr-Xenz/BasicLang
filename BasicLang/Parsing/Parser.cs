using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements;
using System.IO;
using static BasicLang.AbstractTree.TokenType;

namespace BasicLang.Parsing;

internal partial class Parser
{
    private readonly IReadOnlyList<Token> _tokens;
    private readonly ExpressionParser _expressionParser;

    private int _position;

    private readonly List<ProgramError> _parsingErrors = new();
    private readonly string _sourceCode;

    public Parser(IEnumerable<Token> tokens, string sourceCode)
    {
        _tokens = tokens.Where(t => t.Type != Comment).ToArray();
        _sourceCode = sourceCode;
        _expressionParser = new(this);
    }

    public ParseSyntaxTree Parse()
    {
        return new ParseSyntaxTree(ParseStatement(), _parsingErrors, _sourceCode);
    }

    private IStatement ParseStatement()
    {
        while (_position < _tokens.Count)
        {
            var current = Peek();
            if (current.Type == EoL)
                continue;

            return current.Type switch
            {
                Program => ParseProgramDeclaration(current),
                Let => ParseLetVariableDeclarationExpression(current),
                Goto => ParseGoto(current),
                _ => ParseUnknown(current),
            };
        }

        return new ErrorStatement(string.Empty, default);
    }

    private IStatement ParseGoto(Token current)
    {
        MovePosition();
        var lineNumber = Peek();

        if (lineNumber.Type == Number)
        {
            MovePosition();
            return new GotoStatement(lineNumber.Value, GetSourcePositionFromRange(current, lineNumber));
        }
        else
        {
            throw new ProgramException("Line number expected", lineNumber.SourcePosition);
        }
    }

    private IStatement ParseProgramDeclaration(Token initial)
    {
        var startPostion = initial.SourcePosition;
        var programName = PeekNext();

        if (!Match(Identifier))
        {
            MovePostitionToEnd();
            var error = "Program identifier expected";

            AddError(error, programName.SourcePosition);
            return new ErrorStatement(error, programName.SourcePosition);
        }

        MovePosition(2);
        var children = ParseUntilTokenMet(End);

        var endOfProgram = PeekNext();

        var result = new ProgramStatement(children, GetSourcePositionFromRange(startPostion, endOfProgram.SourcePosition));
        MovePostitionToEnd();
        return result;

        void MovePostitionToEnd()
        {
            _position = _tokens.Count - 1;
        }
    }

    private IStatement ParseUnknown(Token token)
    {
        var errorText = $"Unknown token at {token.SourcePosition}";
        AddError(errorText, token.SourcePosition);
        var result = new ErrorStatement(errorText, token.SourcePosition);

        MovePosition();
        return result;
    }

    private IStatement ParseLetVariableDeclarationExpression(Token initial)
    {
        if (!Match(Identifier))
        {
            MovePosition();
            throw new ProgramException("Identifier expected", Peek().SourcePosition);
        }

        MovePosition();
        var varToken = Peek();
        var expression = _expressionParser.Parse(varToken);
        return new VariableDeclarationStatement(new ExpressionStatement(expression), varToken.Value,
            GetSourcePositionFromRange(initial, varToken), GetSourcePositionFromRange(initial, Peek()));
    }

    private void MovePosition(int step = 1)
    {
        _position += step;
    }

    private Token Peek()
    {
        return _position > _tokens.Count
            ? new Token(EoF, string.Empty, default)
            : _tokens[_position];
    }

    private Token PeekNext()
    {
        return _position + 1 >= _tokens.Count
            ? _tokens[^1]
            : _tokens[_position + 1];
    }

    private IEnumerable<IStatement> ParseUntilTokenMet(TokenType type)
    {
        var result = new List<IStatement>();
        while (!Match(type))
        {
            MovePosition();
            result.Add(ParseStatement());
        }
        return result;
    }

    private void AddError(string text, SourcePosition position)
    {
        _parsingErrors.Add(new ProgramError(text, position));
    }

    private SourcePosition GetSourcePositionFromRange(ICodeElement start, ICodeElement end) =>
        GetSourcePositionFromRange(start.SourcePosition, end.SourcePosition);

    private SourcePosition GetSourcePositionFromRange(SourcePosition start, SourcePosition end) =>
        new(start.Offset, start.Line, start.Column, end.Offset - start.Offset + end.Length);

    private bool Match(TokenType type) => PeekNext().Type == type;

    private bool Match(params TokenType[] types)
    {
        var next = PeekNext().Type;
        return types.Any(t => t == next);
    }

    private void Skip(int step = 1) => _position += step;

    private Token Consume()
    {
        var result = Peek();
        Skip();
        return result;
    }

}
