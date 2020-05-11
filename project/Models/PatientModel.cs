using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Observer;

namespace FHIR_FIT3077.Models
{
    public class PatientModel : Subject
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<RecordModel> Records { get; set; }

    }
}
