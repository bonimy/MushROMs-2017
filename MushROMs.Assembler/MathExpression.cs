namespace MushROMs.Assembler
{
    public class MathExpression
    {
        public Calculation Calculation
        {
            get;
            internal set;
        }

        public BinaryOperator BinaryOperator
        {
            get;
            private set;
        }

        public MathExpression(UnaryOperator unaryOperator, Calculation calculation, BinaryOperator binaryOperator)
        {
            var unary = new UnaryExpression(calculation, unaryOperator);
            Calculation = unary.Result;
            BinaryOperator = binaryOperator;
        }
    }
}
