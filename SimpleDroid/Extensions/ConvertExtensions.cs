using System;

namespace SimpleDroid
{
    static class ConvertExtensions
    {
        public static int ToInt(this object source)
        {
            if ( source is int) return (int) source;

            if (source is string)
            {
                return ((string)source).ToInt();
            }

            throw new NotImplementedException();
        }

        public static int ToInt(this string source)
        {
            int value;
            int.TryParse(source, out value);
            return value;
        }
    }
}