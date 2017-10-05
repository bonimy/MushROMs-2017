using System.Diagnostics;
using System.Text;

namespace Helper.PixelFormats
{
    /// <summary>
    /// Represents a <see cref="Byte"/> with an indexer for each bit value.
    /// </summary>
    /// <threadsafety static="true" instance="false"/>
    [DebuggerDisplay("{Value}")]
    public struct BitArray
    {
        /// <summary>
        /// The size, in bytes, of <see cref="BitArray"/>.
        /// This field is constant.
        /// </summary>
        public const int SizeOf = sizeof(byte);

        /// <summary>
        /// The number of bits contained in a single <see cref="Byte"/>.
        /// This field is constant.
        /// </summary>
        public const int BitsPerByte = 8;

        /// <summary>
        /// Gets or sets the value of this <see cref="BitArray"/>.
        /// </summary>
        private byte Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bit value at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the bit to get or set.
        /// </param>
        /// <returns>
        /// 1 if the bit is set; otherwise 0.
        /// </returns>
        /// <remarks>
        /// Setting a bit to a non-zero value will set the bit to 1.
        /// <para/><para/>
        /// If <paramref name="index"/> is outside of the bit range, 0
        /// is returned and nothing is set.
        /// </remarks>
        public unsafe byte this[int index]
        {
            get => (byte)((Value >> index) & 1);
            set
            {
                if (value == 0)
                    Value &= (byte)(~(1 << index));
                else
                    Value |= (byte)(1 << index);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> class from a specified
        /// <see cref="Byte"/> value.
        /// </summary>
        /// <param name="value">
        /// A <see cref="Byte"/> that specifies <see cref="Value"/>.
        /// </param>
        private BitArray(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Converts the specified <see cref="Byte"/> data type to a 
        /// <see cref="BitArray"/> structure.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Byte"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="BitArray"/> that results from the conversion.
        /// </returns>
        public static implicit operator BitArray(byte value)
        {
            return new BitArray(value);
        }
        /// <summary>
        /// Converts the specified <see cref="BitArray"/> structure to a 
        /// <see cref="Byte"/> data type.
        /// </summary>
        /// <param name="array">
        /// The <see cref="BitArray"/> to be converted.
        /// </param>
        /// <returns>
        /// The <see cref="Byte"/> that results from the conversion.
        /// </returns>
        public static implicit operator byte(BitArray array)
        {
            return array.Value;
        }

        /// <summary>
        /// Compares two <see cref="BitArray"/> objects. The result specifies
        /// whether <see cref="Value"/> of the two <see cref="BitArray"/>
        /// objects is equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="BitArray"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="BitArray"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Value"/> of <paramref name="left"/> and <paramref name="right"/>
        /// is equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(BitArray left, BitArray right)
        {
            return left.Value == right.Value;
        }
        /// <summary>
        /// Compares two <see cref="BitArray"/> objects. The result specifies
        /// whether <see cref="Value"/> of the two <see cref="BitArray"/>
        /// objects is unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="BitArray"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="BitArray"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Value"/> of <paramref name="left"/> and <paramref name="right"/>
        /// is unequal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(BitArray left, BitArray right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Specifies whether this <see cref="BitArray"/> is the same value as
        /// the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object"/> to test.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is a <see cref="BitArray"/> and has the same
        /// <see cref="Value"/> as this <see cref="BitArray"/>; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BitArray))
                return false;

            return (BitArray)obj == this;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="BitArray"/>.
        /// </summary>
        /// <returns>
        /// An integer values that specifies a hash value for this <see cref="BitArray"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        /// <summary>
        /// Converts this <see cref="BitArray"/> to a human-readable <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> that represents this <see cref="BitArray"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < BitsPerByte; i++)
                sb.Append((char)(this[i] + '0'));
            return sb.ToString();
        }
    }
}
