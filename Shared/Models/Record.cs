namespace FIT3077.Shared.Models
{
    public class Record
    {
        public RecordType Type { get; set; }
        public string Value { get; set; }
        public string Date { get; set; }

    }
    public enum RecordType
    {
        Cholesterol,
        DiastolicPressure,
        SystolicPressure
    }
}