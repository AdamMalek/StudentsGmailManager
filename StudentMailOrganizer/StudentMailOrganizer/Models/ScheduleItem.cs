using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
