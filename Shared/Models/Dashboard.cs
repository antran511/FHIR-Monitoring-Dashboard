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
            HighCholFlag();
            HighBloodPressureFlag();
        }

        public void HighCholFlag()
        {
            double totalChol = 0.0;
            for (int i = 0; i < Monitors.Count; i++)
            {
                var monitor = Monitors[i].MeasurementList.CholesterolRecords[0].Value;
                var val = Convert.ToDouble(monitor);
                totalChol += val;
            }

            var averageChol = totalChol / Monitors.Count;
            foreach(var t in Monitors)
            {
                var val = Convert.ToDouble(t.MeasurementList.CholesterolRecords[0].Value);
                if (val > averageChol)
                {
                    t.CholFlag = true;
                }
            }
        }

        public void HighBloodPressureFlag()
        {
            double totalChol = 0.0;
            for (int i = 0; i < Monitors.Count; i++)
            {
                var monitor = Monitors[i].MeasurementList.CholesterolRecords[0].Value;
                var val = Convert.ToDouble(monitor);
                totalChol += val;
            }

            var averageChol = totalChol / Monitors.Count;
            foreach (var t in Monitors)
            {
                var val = Convert.ToDouble(t.MeasurementList.CholesterolRecords[0].Value);
                if (val > averageChol)
                {
                    t.CholFlag = true;
                }
            }

        }
    }
}
