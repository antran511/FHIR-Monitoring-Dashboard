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

        private SysDiastolicThreshold SysDiastolicValues { get; set; } = new SysDiastolicThreshold();

        public void RegisterMonitor(Monitor monitor)
        {
            Monitors ??= new List<Monitor>();
            Monitors.Add(monitor);
            HighCholFlag();
            HighBloodPressureFlag(SysDiastolicValues);
        }

        public void HighCholFlag()
        {
            double totalChol = 0.0;
            foreach (var m in Monitors)
            {
                var monitor = m.MeasurementList.CholesterolRecords.Records[0].CholesterolValue;
                var val = Convert.ToDouble(monitor);
                totalChol += val;
            }

            var averageChol = totalChol / Monitors.Count;
            foreach(var t in Monitors)
            {
                var val = Convert.ToDouble(t.MeasurementList.CholesterolRecords.Records[0].CholesterolValue);
                t.CholFlag = val > averageChol;
            }
        }

        public void HighBloodPressureFlag(SysDiastolicThreshold highBloodValues)
        {
            SysDiastolicValues = highBloodValues;
            var systolicValue = SysDiastolicValues.Systolic;
            var diastolicValue = SysDiastolicValues.Diastolic;

            foreach (var t in Monitors)
            {
                var systolicMonitor = int.Parse(t.MeasurementList.BloodPressureRecords.Records[0].SystolicValue);
                var diastolicMonitor = int.Parse(t.MeasurementList.BloodPressureRecords.Records[0].DiastolicValue);
                t.SystolicFlag = systolicMonitor > systolicValue;
                t.DiastolicFlag = diastolicMonitor > diastolicValue;
            }
            
        }


        public void DeregisterMonitor(Monitor monitor)
        {
            Monitors?.Remove(Monitors.SingleOrDefault(m => m.PatientId == monitor.PatientId));
            HighCholFlag();
        }
    }
}
