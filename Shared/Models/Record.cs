namespace FIT3077.Shared.Models
{
    public class Record
    {
        public string Date { get; set; }
        public bool IsMonitored { get; set; } = true;


        public void ChangeIsMonitoredValue()
        {
            IsMonitored = !IsMonitored;
        }
    }
    
}