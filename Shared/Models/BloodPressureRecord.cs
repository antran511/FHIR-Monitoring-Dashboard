using System;

namespace FIT3077.Shared.Models
{
    public class BloodPressureRecord 
    {

        public Record SystolicRecord { get; set; }

        public Record DiastolicRecord { get; set; }
        public string Date => SystolicRecord.Date;
    }
}