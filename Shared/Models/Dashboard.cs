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
            double totalSystolic = 0.0;
            double totalDiastolic = 0.0;
            for (int i = 0; i < Monitors.Count; i++)
            {
                var systolic = Monitors[i].MeasurementList.BloodPressureRecords[0].SystolicRecord.Value;
                var systolicVal = Convert.ToDouble(systolic);

                var diastolic = Monitors[i].MeasurementList.BloodPressureRecords[0].DiastolicRecord.Value;
                var diastolicVal = Convert.ToDouble(diastolic);

                totalSystolic += systolicVal;
                totalDiastolic += diastolicVal;
            }

            var averageSystolic = totalSystolic / Monitors.Count;
            var averageDiastolic = totalDiastolic / Monitors.Count;
            foreach (var t in Monitors)
            {
                var systolicVal = Convert.ToDouble(t.MeasurementList.BloodPressureRecords[0].SystolicRecord.Value);
                var diastolicVal = Convert.ToDouble(t.MeasurementList.BloodPressureRecords[0].DiastolicRecord.Value);
                if (systolicVal > averageSystolic)
                {
                    t.SystolicFlag = true;
                }
                if (diastolicVal > averageDiastolic)
                {
                    t.DiastolicFlag = true;
                }
            }

        }


        public void DeregisterMonitor(string monitorId)
        {
            Monitors?.Remove(Monitors.SingleOrDefault(m => m.PatientId == monitorId));
            HighCholFlag();
            HighBloodPressureFlag();
        }
    }
}
