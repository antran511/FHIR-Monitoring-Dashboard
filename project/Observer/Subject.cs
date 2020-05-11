using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.Observer
{
    public class Subject : IObservable<PatientModel>
    {
        private List<IObserver<PatientModel>> observers;

        public Subject()
        {
            observers = new List<IObserver<PatientModel>>();
        }

        public IDisposable Subscribe(IObserver<PatientModel> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                
            }
            return new UnSubscriber(observers, observer);
        }
        private class UnSubscriber : IDisposable
        {
            private List<IObserver<PatientModel>> lstObservers;
            private IObserver<PatientModel> observer;

            public UnSubscriber(List<IObserver<PatientModel>> ObserversCollection,
                IObserver<PatientModel> observer)
            {
                this.lstObservers = ObserversCollection;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (this.observer != null)
                {
                    lstObservers.Remove(this.observer);
                }
            }
        }

        private void MeasurementsChanged(PatientModel patient)
        {
            foreach (var obs in observers)
            {
                obs.OnNext(patient);
            }
        }
        public void SetMeasurements(PatientModel patient)
        {
            MeasurementsChanged(patient);
        }

    }
}
