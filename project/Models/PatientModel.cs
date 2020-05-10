using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIR_FIT3077.Models
{
    public class PatientModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Measurement> MeasurementList { get; set; }
    }
}
