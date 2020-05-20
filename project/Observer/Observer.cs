using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using Newtonsoft.Json;

namespace FHIR_FIT3077.Observer
{
    public class Observer : IObserver<PatientModel>
    {
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
            throw new NotImplementedException();
        }

        public void OnNext(PatientModel value)
        {
           
        }
    }
}
