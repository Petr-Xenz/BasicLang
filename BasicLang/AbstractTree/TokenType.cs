namespace BasicLang.AbstractTree
{
    internal enum TokenType
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

        EoL,
        EoF,
        //Begining of keywords
        Program,
        Or,
        Xor,
        And,
        Let,
        Goto,
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
        End,
        //End of keywords
    }
}