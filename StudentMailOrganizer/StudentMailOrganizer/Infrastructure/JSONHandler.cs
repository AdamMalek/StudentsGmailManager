using Newtonsoft.Json;
using StudentMailOrganizer.Models;
using StudentMailOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace StudentMailOrganizer.Infrastructure
{
    public class JSONHandler
    {
        private string _path = ConfigurationManager.AppSettings["JsonFolder"] + "/" + ConfigurationManager.AppSettings["JsonFileName"];
        JsonSerializer _js = new JsonSerializer();
        JsonTextReader _reader;
        JsonTextWriter _writer;

        public JSONHandler()
        {
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

        public bool SaveScheduler(List<ScheduleItem> schedulerItems)
        {
            try
            {
                StreamWriter json = new StreamWriter(_path);
                _writer = new JsonTextWriter(json);
                ScheduleStoreModel data = new ScheduleStoreModel();
                data.Items = schedulerItems;
                data.Id = Guid.NewGuid();
                _js.Serialize(_writer, data);
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
