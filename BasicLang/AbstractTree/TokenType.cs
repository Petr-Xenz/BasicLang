namespace BasicLang.AbstractTree
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

        EoL,
        EoF,
    }
}