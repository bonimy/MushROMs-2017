namespace Helper
{
    /// <summary>
    /// Provides static methods for generating hash codes for structs containing recurring sets of values.
    /// </summary>
    public static class Hash
    {
        public static int Generate(int value)
        {
            return value;
        }

        public static int Generate(int value1, int value2)
        {
            return value1 ^ value2;
        }

        public static int Generate(int value1, int value2, int value3)
        {
            return unchecked((int)((uint)value1 ^
                        (((uint)value2 << 8) | ((uint)value2 >> 16)) ^
                        (((uint)value3 << 16) | ((uint)value3 >> 8))));
        }

        public static int Generate(int value1, int value2, int value3, int value4)
        {
            return unchecked((int)((uint)value1 ^
                        (((uint)value2 << 13) | ((uint)value2 >> 19)) ^
                        (((uint)value3 << 26) | ((uint)value3 >> 6)) ^
                        (((uint)value4 << 7) | ((uint)value4 >> 25))));
        }

        public static int Generate(int value1, int value2, int value3, int value4, int value5)
        {
            return unchecked((int)((uint)value1 ^
                        (((uint)value2 << 24) | ((uint)value2 >> 4)) ^
                        (((uint)value3 << 16) | ((uint)value3 >> 8)) ^
                        (((uint)value4 << 8) | ((uint)value4 >> 16)) ^
                        (((uint)value5 << 4) | ((uint)value5 >> 24))));
        }

        internal static int SingleCode(float value)
        {
            unsafe
            {
                var ptr = &value;
                return *(int*)ptr;
            }
        }

        public static int Generate(float value)
        {
            return Generate(SingleCode(value));
        }

        public static int Generate(float value1, float value2)
        {
            return Generate(SingleCode(value1), SingleCode(value2));
        }

        public static int Generate(float value1, float value2, float value3)
        {
            return Generate(SingleCode(value1), SingleCode(value2),
                SingleCode(value3));
        }

        public static int Generate(float value1, float value2, float value3, float value4)
        {
            return Generate(SingleCode(value1), SingleCode(value2),
                SingleCode(value3), SingleCode(value4));
        }
    }
}
