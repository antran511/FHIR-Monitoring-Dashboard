using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.Observer
{
    public class Subscriber : IObserver<PatientModel>
    {
        private PatientModel patient;
        private IDisposable unsubscriber;
        public Subscriber(IObservable<PatientModel> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }
        public void Subscribe(IObservable<PatientModel> provider)
        {
            if (unsubscriber == null)
            {
                unsubscriber = provider.Subscribe(this);
            }
        }
       

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
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
            this.patient.MeasurementList = value.MeasurementList;
        }
    }
}
