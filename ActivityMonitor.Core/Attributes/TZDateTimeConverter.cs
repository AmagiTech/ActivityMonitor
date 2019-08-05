using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityMonitor.Core.Attributes
{
    public class TZDateTimeConverter : IsoDateTimeConverter
    {
        public TZDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";
        }
    }
}
