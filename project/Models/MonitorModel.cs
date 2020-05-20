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
    public class MonitorModel : IObserver<PatientModel>
    {
        //JsonProperty added for deserialization of private field
        [JsonProperty]
        public string Id { get; private set; }
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public string Birthdate { get; private set; }
        [JsonProperty]
        public AdministrativeGender? Gender { get; private set; }
        [JsonProperty]
        public string Address { get; private set; }
        [JsonProperty]
        public string City { get; private set; }
        [JsonProperty]
        public string State { get; private set; }
        [JsonProperty]
        public string Country { get; private set; }
        [JsonProperty]
        public List<RecordModel> Records { get; private set; }
        [JsonIgnore]
        private IDisposable _unsubscriber;
       
        public void Subscribe(IObservable<PatientModel> provider)
        {
            _unsubscriber ??= provider.Subscribe(this);
        }
       

        public void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(PatientModel value)
        {
            this.Id = value.Id;
            this.Name = value.Name;
            this.Birthdate = value.Birthdate;
            this.Gender = value.Gender;
            this.Address = value.Address;
            this.City = value.City;
            this.State = value.State;
            this.Country = value.Country;
            this.Records = value.Records;
        }

       

        
    }
}
