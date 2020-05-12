using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.Observer
{
    public class PatientMonitorModel : IObserver<PatientModel>
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public RecordModel Record { get; private set; }
        private IDisposable _unsubscriber;
        public PatientMonitorModel()
        {
        }
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
            this.Record = value.Record;
        }

       

        
    }
}
