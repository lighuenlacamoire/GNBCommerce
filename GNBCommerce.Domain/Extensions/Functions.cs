using System;
using System.Collections.Generic;
using System.Text;

namespace GNBCommerce.Domain.Extensions
{
    public static class Functions
    {
        public static bool StringCompare(string origin, string final)
        {
            return origin.Equals(final, StringComparison.InvariantCultureIgnoreCase);
        }
        public static string ExchangeConversion(string from, string to)
        {
            float origin = float.Parse(from);
            float intermediate = float.Parse(to);

            return string.Format("{0:f2}", (origin * intermediate));
        }
    }
}
