using ActivityMonitor.Core.Attributes;
using ActivityMonitor.Core.ExtensionMethods;
using Newtonsoft.Json;
using System;

namespace ActivityMonitor.Core
{
    public class ActivityLog
    {
       
        [JsonProperty(PropertyName = "timestamp")]
        [JsonConverter(typeof(TZDateTimeConverter))]
        public DateTime ActivationTime { get; set; }
        [JsonProperty(PropertyName = "foregroundApp")]
        public string ForegroundApp { get; set; }
        [JsonProperty(PropertyName = "backgroundApps")]
        public string[] BackgroundApps { get; set; }
      
        public ActivityLog(Activity activity, string[] backgroundActivities)
        {
            ActivationTime = activity.ActivationTime;
            ForegroundApp = activity.ProcessName;
            BackgroundApps = backgroundActivities;
        }
        public ActivityLog()
        {

        }

        
    }
}
