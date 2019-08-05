using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ActivityMonitor.Core.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToStringTZ(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
        public static DateTime ToDateTimeTZ(this string str)
            => DateTime.ParseExact(str, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
    }
}
