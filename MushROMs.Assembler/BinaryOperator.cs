namespace MushROMs.Assembler
{
    public enum BinaryOperator
    {
        None,
        Addition = TokenType.Addition,
        Subtraction = TokenType.Subtraction,
        Multiplication = TokenType.Multiplication,
        Division = TokenType.Division,
        Modulo = TokenType.Modulo,
        BitShiftLeft = TokenType.BitShiftLeft,
        BitShiftRight = TokenType.BitShiftRight,
        BitwiseAND = TokenType.BitwiseAND,
        BitwiseOR = TokenType.BitwiseOR,
        BitwiseXOR = TokenType.BitwiseXOR
    }
}
