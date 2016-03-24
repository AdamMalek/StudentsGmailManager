using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;

namespace StudentMailOrganizer.ViewModels
{
    public class ScheduleStoreModel
    {
        public Guid Id { get; set; }
        public List<ScheduleItem> Items { get; set; }
    }
}
