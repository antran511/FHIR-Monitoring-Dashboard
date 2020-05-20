using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using Hl7.Fhir.Model;
using Newtonsoft.Json;

namespace FHIR_FIT3077.Observer
{
    [Serializable()]
    public class MonitorModel : Observer
    {
        //JsonProperty added for deserialization of private field
        [JsonProperty]
        public string Id { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public List<RecordModel> Records { get; private set; }

        public new void OnNext(PatientModel value)
        {
            this.Id = value.Id;
            this.Name = value.Name;
            this.Records = value.Records;
        }

       

        
    }
}
