﻿namespace BasicLang.AbstractTree
{
    public enum TokenType
    {
        None,
        Unknown,

        Number,
        Boolean,

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
    }
}