using System;
using System.Collections.Generic;
using System.Text;

namespace FIT3077.Shared.Models
{
    public abstract class RecordList
    {
        public List<Record> Records { get; set; }
        public bool IsMonitored { get; set; } = true;
        public void ChangeMonitoredState() {
            IsMonitored = !IsMonitored;
        }
    }
}
