using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;

namespace FHIR_FIT3077.ViewModels
{
    public class MonitorViewModel
    {
        public List<PatientMonitorModel> MonitorList { get; set; }
    }
}
