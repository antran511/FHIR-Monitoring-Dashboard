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

        public SysDiastolicThreshold SysDiastolicValues { get; set; }

        public void RegisterMonitor(Monitor monitor)
        {
            Monitors ??= new List<Monitor>();
            Monitors.Add(monitor);
            HighCholFlag();
            HighBloodPressureFlag();
        }

        public void HighCholFlag()
        {
            double totalChol = 0.0;
            for (int i = 0; i < Monitors.Count; i++)
            {
                var monitor = Monitors[i].MeasurementList.CholesterolRecords[0].CholesterolValue;
                var val = Convert.ToDouble(monitor);
                totalChol += val;
            }

            var averageChol = totalChol / Monitors.Count;
            foreach(var t in Monitors)
            {
                var val = Convert.ToDouble(t.MeasurementList.CholesterolRecords[0].CholesterolValue);
                if (val > averageChol)
                {
                    t.CholFlag = true;
                }
                else
                {
                    t.CholFlag = false;
                }
            }
        }

        public void HighBloodPressureFlag()
        {
            var systolicValue = SysDiastolicValues.Systolic;
            var diastolicValue = SysDiastolicValues.Diastolic;

            foreach (var t in Monitors)
            {
                var systolicMonitor = int.Parse(t.MeasurementList.BloodPressureRecords[0].SystolicValue);
                var diastolicMonitor = int.Parse(t.MeasurementList.BloodPressureRecords[0].DiastolicValue);
                if (systolicMonitor > systolicValue)
                {
                    t.SystolicFlag = true;
                }
                else
                {
                    t.SystolicFlag = false;
                }
                if (diastolicMonitor > diastolicValue)
                {
                    t.DiastolicFlag = true;
                }
                else
                {
                    t.DiastolicFlag = false;
                }
            }
            
        }


        public void DeregisterMonitor(Monitor monitor)
        {
            Monitors?.Remove(Monitors.SingleOrDefault(m => m.PatientId == monitor.PatientId));
            HighCholFlag();
        }
    }
}
