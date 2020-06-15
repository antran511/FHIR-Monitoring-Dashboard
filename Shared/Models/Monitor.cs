using System.Collections.Generic;

namespace FIT3077.Shared.Models
{
    public class Monitor
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public List<Measurement> MeasurementList { get; set; }

        public Monitor(string patientId, string patientName)
        {
            this.PatientId = patientId;
            this.PatientName = patientName;
        }

    }
}