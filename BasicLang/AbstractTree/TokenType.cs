namespace BasicLang.AbstractTree;

internal enum TokenType
{
    None,
    Unknown,

    Number,
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
    // Begining of keywords
    Program,
    Or,
    Xor,
    And,
    Not,
    Let,
    Goto,
    Print,
    Input,
    For,
    To,
    Step,
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
    FnEnd,
    Dim,
    True,
    False,
    End,
    // End of keywords
}