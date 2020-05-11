using System;

namespace FHIR_FIT3077.Models
{
    public class Measurement
    {
        public MeasurementType Type { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }
    }

    public enum MeasurementType
    {
        Cholesterol,
        Glucose
    }
}