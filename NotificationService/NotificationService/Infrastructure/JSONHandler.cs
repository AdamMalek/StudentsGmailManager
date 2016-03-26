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
            if (!Directory.Exists(ConfigurationManager.AppSettings["JsonFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["JsonFolder"]);
            }
        }

        public List<ScheduleItem> LoadScheduler()
        {
            if (File.Exists(_path))
            {
                try
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
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(1000);
                    return LoadScheduler();
                }
            }
            else
            {
                File.Create(_path).Close();
                return new List<ScheduleItem>();
            }
        }        
    }
}
