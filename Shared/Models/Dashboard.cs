using System;
using System.Collections.Generic;
using System.Text;

namespace FIT3077.Shared.Models
{
    public class Dashboard
    {
        public IReadOnlyDictionary<string, Patient> Patients { get; set; }
        public List<Monitor> Monitors { get; } = new List<Monitor>();

        public void RegisterMonitor(Monitor monitor)
        {
            Monitors.Add(monitor);
        }
    }
}
