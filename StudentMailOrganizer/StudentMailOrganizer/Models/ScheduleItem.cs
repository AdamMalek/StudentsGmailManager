using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web.Script.Serialization;

namespace StudentMailOrganizer.Models
{
    public class ScheduleItem
    {
        [JsonProperty(ItemConverterType = typeof(JavaScriptDateTimeConverter))]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [ScriptIgnore]
        public string Time
        {
            get
            {                
                return Date.ToString("HH:mm");
            }
        }
    }
}
