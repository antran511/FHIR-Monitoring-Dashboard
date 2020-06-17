using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIT3077.Shared.Models
{
    public class Dashboard
    {
        public Dictionary<string, Patient> Patients { get; set; }
        public List<Monitor> Monitors { get; set; }

        public void RegisterMonitor(Monitor monitor)
        {
            Monitors ??= new List<Monitor>();
            Monitors.Add(monitor);
        }

        public void DeregisterMonitor(string monitorId)
        {
            Monitors?.Remove(Monitors.SingleOrDefault(m => m.PatientId == monitorId));
        }
    }
}
