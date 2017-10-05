namespace MushROMs.Assembler
{
    public enum UnaryOperator
    {
        None,
        Positive = TokenType.Addition,
        Negative = TokenType.Subtraction,
        Negation = TokenType.Negation
    }
}
