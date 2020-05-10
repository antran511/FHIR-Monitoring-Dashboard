using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.ViewModels
{
    public class MonitorViewModel : IObservable<PatientModel>
    {
        private List<IObserver<PatientModel>> observers;

        public MonitorViewModel()
        {
            observers = new List<IObserver<PatientModel>>();
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<PatientModel>> _observers;
            private IObserver<PatientModel> _observer;

            public Unsubscriber(List<IObserver<PatientModel>> observers, IObserver<PatientModel> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }
        public Dictionary<string, PatientModel> MonitorList { get; set; }

        public IDisposable Subscribe(IObserver<PatientModel> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }
    }
}
