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
            {
                Skip();
                continue;
            }

            return current.Type switch
            {
                Program => ParseProgramDeclaration(current),
                Let => ParseLetVariableDeclarationExpression(current),
                Goto => ParseGoto(current),
                Print => ParsePrint(current),
                Input => ParseInput(current),
                If => ParseIf(current),
                For => ParseFor(current),
                While => ParseWhile(current),
                Do => ParseDoUntil(current),
                Def => ParseFunction(current),
                Identifier => ParseIdentifier(current),
                _ => ParseUnknown(current),
            };
        }

        return new ErrorStatement(string.Empty, default);
    }

    private IStatement ParseFunction(Token current)
    {
        Skip(); //Def
        Expect(Identifier);
        var functionName = Consume();
        
        Expect(OpenParenthesis);
        Skip(); //(
        var arguments = !Match(CloseParenthesis) ? ParseExpressionsList(Peek()) : Enumerable.Empty<IExpression>();
        Expect(CloseParenthesis);
        Skip(); //)
        Expect(EoL);

        var (innerStatements, end) = ParseStatementsUntilTokenIsMet(FnEnd);

        return new FunctionStatement(functionName, arguments, innerStatements, GetSourcePositionFromRange(current, end), current.SourcePosition);
    }

    private IStatement ParseDoUntil(Token current)
    {
        Skip(); //Do
        Expect(EoL);
        var (innerStatements, end) = ParseStatementsUntilTokenIsMet(Until);
        var condition = _expressionParser.Parse(Peek());

        return new DoUntilStatement(condition, innerStatements, GetSourcePositionFromRange(current, end));
    }

    private IStatement ParseWhile(Token current)
    {
        Skip(); //while
        var condition = _expressionParser.Parse(Peek());
        Expect(EoL);
        var (innerStatements, end) = ParseStatementsUntilTokenIsMet(Loop);

        return new WhileStatement(condition, innerStatements, GetSourcePositionFromRange(current, end));
    }

    private IStatement ParseFor(Token current)
    {
        Skip(); //for

        ForCounterExpression? counter = null;
        if (Match(Identifier))
        {
            counter = ParseCounter();
        }
        Expect(EoL);

        var (innerStatements, end) = ParseStatementsUntilTokenIsMet(Next);

        return new ForStatement(counter, innerStatements, GetSourcePositionFromRange(current, end));

        ForCounterExpression ParseCounter()
        {
            var counterVariable = _expressionParser.Parse(Peek());
            Expect(To);
            Skip(); //to
            var limit = _expressionParser.Parse(Peek());
            var step = 1L;
            if (Match(Step))
            {
                Skip(); //Step
                Expect(Number);
                if (Peek().Value.Contains('.'))
                    throw new ProgramException("Integer literal expected", Peek());
                step = long.Parse(Consume().Value);
            }

            return new ForCounterExpression(counterVariable, limit, step, GetSourcePositionFromRange(counterVariable, Peek()));

        }
    }

    private (IEnumerable<IStatement> statements, Token end) ParseStatementsUntilTokenIsMet(TokenType type)
    {
        var result = new List<IStatement>();
        while (!Match(type))
        {
            result.Add(ParseStatement());
            SkipInsignificant();
        }
        var end = Consume(); //type

        return (result, end);
    }
    private IStatement ParseIf(Token current)
    {
        var (condition, onTrueStatement) = ParseIfStatement();

        var elseIfStatements = new List<ElseIfStatement>();
        while (Match(ElseIf))
        {
            var start = Peek();
            var (conditionElseIf, onTrueStatementElseIf) = ParseIfStatement();
            elseIfStatements.Add(new ElseIfStatement(conditionElseIf, onTrueStatementElseIf, GetSourcePositionFromRange(start, Peek())));
        }

        IStatement? onFalseStatement = null;
        if (Match(Else))
        {
            Skip(); //else
            onFalseStatement = ParseStatement();
        }

        return new IfStatement(condition, onTrueStatement, onFalseStatement, elseIfStatements, GetSourcePositionFromRange(current, Peek()));

        (IExpression condition, IStatement onTrueStatement) ParseIfStatement()
        {
            Skip(); //if
            var condition = _expressionParser.Parse(Peek());
            if (!Match(Then))
            {
                throw new ProgramException("Then keyword expected", PeekNext());
            }

            Skip(); //then

            var onTrueStatement = ParseStatement();

            return (condition, onTrueStatement);
        }
    }

    private IStatement ParseInput(Token current)
    {
        Skip();
        var expressions = ParseExpressionsList(current);
        return new InputStatement(expressions, GetSourcePositionFromRange(current, Peek()));
    }

    private IStatement ParsePrint(Token current)
    {
        Skip();
        var expressions = ParseExpressionsList(current);
        return new PrintStatement(expressions, GetSourcePositionFromRange(current, Peek()));
    }

    private IEnumerable<IExpression> ParseExpressionsList(Token initial)
    {
        var expressions = new List<IExpression>
        {
            _expressionParser.Parse(Peek())
        };

        while (Match(Comma, Semicolon))
        {
            Skip();
            expressions.Add(_expressionParser.Parse(Peek()));
        }

        if (expressions.Count == 0)
        {
            _parsingErrors.Add(new ProgramError("Print statement should have atlest one expression", initial.SourcePosition));
        }

        return expressions;
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
            return ParseVariableAssignment(current);
        }
    }

    private IStatement ParseGoto(Token current)
    {
        Skip();
        var lineLabel = Peek();

        if (lineLabel.Type == Number || lineLabel.Type == Identifier)
        {
            Skip();
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
        Skip(); //program
        var programName = Peek();

        if (!Match(Identifier))
        {
            MovePostitionToEnd();
            var error = "Program identifier expected";

            AddError(error, programName.SourcePosition);
            return new ErrorStatement(error, programName.SourcePosition);
        }

        Skip();
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

        Skip();
        return result;
    }

    private IStatement ParseLetVariableDeclarationExpression(Token initial)
    {
        Skip(); //let
        if (!Match(Identifier))
        {
            Skip();
            throw new ProgramException("Identifier expected", Peek().SourcePosition);
        }

        return ParseVariableAssignment(initial);
    }

    private IStatement ParseVariableAssignment(Token initial)
    {
        var varToken = Peek();
        var expression = _expressionParser.Parse(varToken);
        return new VariableDeclarationStatement(new ExpressionStatement(expression), varToken.Value,
            GetSourcePositionFromRange(initial, varToken), GetSourcePositionFromRange(initial, Peek()));
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
        while (!MatchNext(type))
        {
            Skip();
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

    private bool Match(TokenType type) => Peek().Type == type;

    private bool MatchNext(TokenType type) => PeekNext().Type == type;

    private void Expect(TokenType type)
    {
        if (!Match(type))
            throw new ProgramException($"Unexpected token {type}", Peek());
    }

    private bool Match(params TokenType[] types)
    {
        var next = Peek().Type;
        return types.Any(t => t == next);
    }

    private void Skip(int step = 1) => _position += step;

    private int SkipInsignificant()
    {
        var steps = 0;
        while(Peek().Type == EoL || Peek().Type == Comment)
        {
            steps++;
            _position++;
        }
        return steps;
    }

    private Token Consume()
    {
        var result = Peek();
        Skip();
        return result;
    }
}