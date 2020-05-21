using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Observer;
using JsonSubTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FHIR_FIT3077.Models
{
    [Serializable()]

    public class PatientModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<RecordModel> Records { get; set; }
       
    }
}
