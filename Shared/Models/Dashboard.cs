﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace FIT3077.Shared.Models
{
    public class Dashboard
    {
        public Dictionary<string, Patient> Patients { get; private set; }
        public List<Monitor> Monitors { get; private set; }
        public Timer t { get; set; }
        private SysDiastolicThreshold SysDiastolicValues { get; set; } = new SysDiastolicThreshold();

        public void RegisterMonitor(Patient patient, Measurement measurement)
        {
            Monitor monitor = new Monitor(patient.Id, patient.Name);
            monitor.MeasurementList = measurement;
            Monitors ??= new List<Monitor>();
            Monitors.Add(monitor);
            ChangePatientMonitorState(patient);
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
            var patient = Patients.SingleOrDefault(p => p.Value.Id == monitor.PatientId).Value;
            Monitors?.Remove(Monitors.SingleOrDefault(m => m.PatientId == monitor.PatientId));
            patient.ChangePatientMonitorState();
            HighCholFlag();
        }

        public void ChangePatientMonitorState(Patient patient)
        {
            patient.ChangePatientMonitorState();
        }

        public void UpdateMeasurement(Monitor monitor, Measurement measurementList)
        {
            var bloodPressureState = monitor.MeasurementList.BloodPressureRecords.IsMonitored;
            var cholesterolState = monitor.MeasurementList.CholesterolRecords.IsMonitored;
            monitor.MeasurementList = measurementList;
            monitor.MeasurementList.CholesterolRecords.IsMonitored = cholesterolState;
            monitor.MeasurementList.BloodPressureRecords.IsMonitored = bloodPressureState;
        }

        public void FetchPatientList(Dictionary<string, Patient> patients)
        {
            Patients = patients;
        }

        public void ClearMonitor()
        {
            Monitors?.Clear();
        }
    }
}
