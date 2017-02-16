using System;

namespace Droid.Core
{
    static class ConvertionExtensions
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