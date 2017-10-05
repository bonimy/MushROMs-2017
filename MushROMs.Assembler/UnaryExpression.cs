using System;
using System.Collections.Generic;

namespace MushROMs.Assembler
{
    public delegate int UnaryOperation(int value);

    public class UnaryExpression
    {
        private static Dictionary<UnaryOperator, UnaryOperation> _unaryOperations;
        private static Dictionary<UnaryOperator, UnaryOperation> UnaryOperations
        {
            get
            {
                if (_unaryOperations == null)
                {
                    _unaryOperations = new Dictionary<UnaryOperator, UnaryOperation>();
                    _unaryOperations.Add(UnaryOperator.None, x => x);
                    _unaryOperations.Add(UnaryOperator.Positive, x => +x);
                    _unaryOperations.Add(UnaryOperator.Negative, x => -x);
                    _unaryOperations.Add(UnaryOperator.Negation, x => ~x);
                }
                return _unaryOperations;
            }
        }

        public Calculation Value
        {
            get;
            set;
        }

        public UnaryOperation Operation
        {
            get;
            set;
        }

        public Calculation Result => () => Operation(Value());
        public UnaryExpression(Calculation value, UnaryOperator operation) :
            this(value, GetUnaryOperation(operation))
        { }
        public UnaryExpression(Calculation value, UnaryOperation operation)
        {
            Value = value??throw new ArgumentNullException(nameof(value));
            Operation = operation??throw new ArgumentNullException(nameof(operation));
        }

        public static UnaryOperation GetUnaryOperation(UnaryOperator value)
        {
            if (!UnaryOperations.ContainsKey(value))
                return null;

            return UnaryOperations[value];
        }
    }
}
