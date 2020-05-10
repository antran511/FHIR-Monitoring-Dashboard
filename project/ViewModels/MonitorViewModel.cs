using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.ViewModels
{
    public class MonitorViewModel
    {
        public Dictionary<string, PatientModel> MonitorList { get; set; }
    }
}
