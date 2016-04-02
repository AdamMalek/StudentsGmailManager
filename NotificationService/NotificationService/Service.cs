using NotificationService.Models;
using Quartz;
using Quartz.Impl;
using StudentMailOrganizer.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotificationService
{
    public class Service : ServiceBase
    {
        FileSystemWatcher fw;
        JSONHandler jsHnd;
        IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
        public Service()
        {     
            string path = ConfigurationManager.AppSettings["JsonFolder"] + "/" + ConfigurationManager.AppSettings["JsonFileName"];
            jsHnd = new JSONHandler(path);
            fw = new FileSystemWatcher();
            fw.Filter = ConfigurationManager.AppSettings["JsonFileName"];
            fw.NotifyFilter = NotifyFilters.LastWrite;
            fw.Path = ConfigurationManager.AppSettings["JsonFolder"];
            fw.Changed += Fw_Changed;
        }
        
        private void SetScheduler(ScheduleItem item)
        {
            IJobDetail job = JobBuilder.Create<NotificationJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(item.Date)
                .WithDescription(CreateDescription(item))
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }

        private string CreateDescription(ScheduleItem item)
        {
            return item.Date.Ticks + "|" + item.Description;
        }

        List<ScheduleItem> _items;

        protected override void OnStart(string[] args)
        {
            fw.EnableRaisingEvents = true;
            Update();
            scheduler.Start();

            base.OnStart(args);
        }

        public void Run()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            fw.EnableRaisingEvents = false;
            scheduler.PauseAll();
            base.OnStop();
        }

        private void Fw_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                fw.EnableRaisingEvents = false;
                Update();
            }
            finally
            {
                fw.EnableRaisingEvents = true;
            }
        }

        private void Update()
        {
            scheduler.Clear();
            _items = jsHnd.LoadScheduler().OrderBy(x => x.Date).Where(x => x.Date > DateTime.Now).ToList();
            if (_items != null)
            {
                foreach (var item in _items)
                {
                    SetScheduler(item);
                }
            }
        }

        private class NotificationJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                string[] desc = context.Trigger.Description.Split('|');
                long ticks;
                long.TryParse(desc[0], out ticks);
                string date = new DateTime(ticks).ToString("dd.MM.yyyy HH:mm");
                string msg = desc[1];
                MessageBox.Show(msg,date,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void InitializeComponent()
        {
            // 
            // Service
            // 
            this.ServiceName = "NotificationService";

        }
    }
}
