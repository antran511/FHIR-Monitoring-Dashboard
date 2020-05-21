using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.ViewModel
{
    public class PatientViewModel
    {
        public static Dictionary<string, PatientModel> PatientList { get; set; }
        public static List<MonitorModel> MonitorList { get; set; }
    }
}
