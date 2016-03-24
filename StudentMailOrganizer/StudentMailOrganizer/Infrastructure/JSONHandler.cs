using Newtonsoft.Json;
using StudentMailOrganizer.Models;
using StudentMailOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StudentMailOrganizer.Infrastructure
{
    public class JSONHandler
    {
        private string _path;
        JsonSerializer _js = new JsonSerializer();
        JsonTextReader _reader;
        JsonTextWriter _writer;

        public JSONHandler(string path)
        {
            _path = path;
        }

        public List<ScheduleItem> LoadScheduler()
        {
            if (File.Exists(_path))
            {
                StreamReader json = new StreamReader(_path);
                _reader = new JsonTextReader(json);
                var scheduler = _js.Deserialize<List<ScheduleItem>>(_reader);
                json.Close();
                if (scheduler != null)
                {
                    return scheduler;
                }
                else
                {
                    return new List<ScheduleItem>();
                }
            }
            else
            {
                File.Create(_path);
                return new List<ScheduleItem>();
            }
        }

        public bool SaveScheduler(List<ScheduleItem> schedulerItems)
        {
            try
            {
                StreamWriter json = new StreamWriter(_path);
                _writer = new JsonTextWriter(json);
                _js.Serialize(_writer, schedulerItems);
                json.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
