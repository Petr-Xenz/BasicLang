namespace BasicLang.AbstractTree
{
    public enum TokenType
    {
        None,
        Unknown,

        Number,
        Boolean,
        Identifier,
        String,
        Comment,

        Equal,
        LessThen,
        LessThenOrEqual,
        GreaterThen,
        GreaterThenOrEqual,
        NotEqual,

        Addition,
        Subtraction,
        Multiplication,
        Division,

        OpenParenthesis,
        CloseParenthesis,

        Assignment,

        Comma,
        Colon,
        Semicolon,

        Program,
        End,
        Let,
        Print,
        Input,
        For,
        To,
        Next,
        Do,
        While,
        Until,
        Loop,
        If,
        Then,
        ElseIf,
        Else,
        Exit,
        Call,
        Sub,
        Def,
        Declare,
        Dim,

        EoL,
        EoF,
    }
}