using System;

namespace FIT3077.Shared.Models
{
    public class BloodPressureRecord 
    {

        public Record SystolicRecord { get; set; }

        public Record DiastolicRecord { get; set; }
        public string Date => SystolicRecord.Date;

        public BloodPressureRecord (Record systoclicRecord, Record diastolicRecord)
        {
            //If not null, assign variable
            SystolicRecord = systoclicRecord ?? throw new ArgumentNullException(nameof(systoclicRecord));
            DiastolicRecord = diastolicRecord ?? throw new ArgumentNullException(nameof(diastolicRecord));
        }
    }
}