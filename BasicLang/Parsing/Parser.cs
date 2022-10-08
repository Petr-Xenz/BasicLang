using BasicLang.AbstractTree;
using BasicLang.AbstractTree.Statements;
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
                Print => ParsePrint(current),
                If => ParseIf(current),
                Identifier => ParseIdentifier(current),
                _ => ParseUnknown(current),
            };
        }

        return new ErrorStatement(string.Empty, default);
    }

    private IStatement ParseIf(Token current)
    {
        Skip(); //if
        var condition = _expressionParser.Parse(Peek());

        if (!Match(Then))
        {
            throw new ProgramException("Then keyword expected", PeekNext().SourcePosition);
        }

        Skip(2); //expression + then

        var onTrueStatement = ParseStatement();
        IStatement? onFalseStatement = null;
        if (Match(Else))
        {
            Skip(2);
            onFalseStatement = ParseStatement();
        }

        return new IfStatement(condition, onTrueStatement, onFalseStatement, GetSourcePositionFromRange(current, Peek()));
    }

    private IStatement ParsePrint(Token current)
    {
        Skip();
        var expressions = new List<IExpression>
        {
            _expressionParser.Parse(Peek())
        };

        while (Match(Comma, Semicolon))
        {
            Skip(2);
            expressions.Add(_expressionParser.Parse(Peek()));
        }

        if (expressions.Count == 0)
        {
            _parsingErrors.Add(new ProgramError("Print statement should have atlest one expression", current.SourcePosition));
        }

        return new PrintStatement(expressions, GetSourcePositionFromRange(current, Peek()));
    }

    private IStatement ParseIdentifier(Token current)
    {
        if (PeekNext().Type == Colon)
        {
            var colon = Consume();
            if (_position > 0 && _tokens[_position - 1].Type != EoL)
            {
                _parsingErrors.Add(new ProgramError("A line label must be on the beginning of a line", GetSourcePositionFromRange(current, colon)));
            }

            return new LabelStatement(current.Value + colon.Value, current.Value, GetSourcePositionFromRange(current, colon));
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private IStatement ParseGoto(Token current)
    {
        MovePosition();
        var lineLabel = Peek();

        if (lineLabel.Type == Number || lineLabel.Type == Identifier)
        {
            MovePosition();
            return new GotoStatement(lineLabel.Value, GetSourcePositionFromRange(current, lineLabel));
        }
        else
        {
            throw new ProgramException("Line number expected", lineLabel.SourcePosition);
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
        var errorText = $"Unknown token at {token.SourcePosition}: {token.Value}";
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
