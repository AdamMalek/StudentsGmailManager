using Newtonsoft.Json;
using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace StudentMailOrganizer.Infrastructure
{
    public class JSONHandler
    {
        string _path = "";
        JsonSerializer _js = new JsonSerializer();
        JsonTextReader _reader;

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
                var scheduler = _js.Deserialize<ScheduleStoreModel>(_reader);
                json.Close();
                if (scheduler != null)
                {
                    return scheduler.Items;
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
    }
}
