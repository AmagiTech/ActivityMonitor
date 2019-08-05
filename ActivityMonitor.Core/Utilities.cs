using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActivityMonitor.Core
{
    public class IOUtilities
    {
        static string m_SessionFolder = null;
        static string SessionFolder
        {
            get
            {
                if (m_SessionFolder == null)
                    m_SessionFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions\\");
                if (!Directory.Exists(m_SessionFolder))
                    Directory.CreateDirectory(m_SessionFolder);
                return m_SessionFolder;
            }
        }
        public static string JsonFilePath(DateTime dateTime) =>
            Path.Combine(SessionFolder, dateTime.ToString("yyyyMMdd") + ".json");


        public static void WriteToJsonFile(IEnumerable<ActivityLog> list)
        {
            var removeOldList = new List<ActivityLog>();
            var o = new JObject();
            foreach (var item in list.GroupBy(q => q.ActivationTime.Date).OrderBy(q => q.Key))
            {
                var sequences = new JObject();
                sequences["sequences"] = JArray.Parse(JsonConvert.SerializeObject(list.Where(q => item.Key == q.ActivationTime.Date)));
                o[DateTime.Now.ToString("dd'.'MM'.'yyy")] = sequences;
                using (StreamWriter file = File.CreateText(JsonFilePath(item.Key)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, o);
                }
                if (item.Key < DateTime.Today)
                    removeOldList = list.Where(q => item.Key == q.ActivationTime.Date).ToList();
            }
            list = list.Except(removeOldList);
        }
        public static IEnumerable<ActivityLog> LoadJson()
        {
            var filePath = JsonFilePath(DateTime.Today);
            if (File.Exists(filePath))
                using (StreamReader r = new StreamReader(filePath))
                {
                    var jObj = JObject.Parse(r.ReadToEnd())
                               [DateTime.Now.ToString("dd'.'MM'.'yyy")]
                               ["sequences"];

                    return jObj.Select(q => q.ToObject<ActivityLog>()).ToList();
                }
            return new List<ActivityLog>();
        }
    }
}
