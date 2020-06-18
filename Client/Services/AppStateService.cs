using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FIT3077.Shared.Models;

namespace FIT3077.Client.Services
{
    public class AppStateService
    {
        private Dashboard Dashboard { get; } = new Dashboard();
        public IReadOnlyList<Monitor> Monitors => Dashboard.Monitors;
        public IReadOnlyDictionary<string, Patient> Patients => Dashboard.Patients;
        public bool SearchInProgress { get; private set; }
        public int CurrentInterval { get; private set; } = 5000;
        public event Action OnChange;
        public System.Timers.Timer t;

        private readonly HttpClient _http;
        public AppStateService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task Search(string value)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Dashboard.Patients = await _http.GetFromJsonAsync<Dictionary<string, Patient>>($"/api/practitioner/{value}");
            Dashboard.Monitors?.Clear();
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task AddToMonitors(Patient patient)
        {
            Monitor monitor = new Monitor(patient.Id, patient.Name);
            SearchInProgress = true;
            NotifyStateChanged();

            monitor.MeasurementList = await FetchMeasurement(monitor);
            Dashboard.RegisterMonitor(monitor);
            SearchInProgress = false;
            Dashboard.Patients.SingleOrDefault(p => p.Key == monitor.PatientId).Value.IsMonitored = true;
            NotifyStateChanged();
        }

        public void RemoveFromMonitors(Monitor monitor)
        {
            Dashboard.DeregisterMonitor(monitor);
            Dashboard.Patients.SingleOrDefault(p => p.Key == monitor.PatientId).Value.IsMonitored = false;
            NotifyStateChanged();
        }

        public async Task<Measurement> FetchMeasurement(Monitor monitor)
        {
            var id = monitor.PatientId;
            var fetchBloodPressureTask = _http.GetFromJsonAsync<List<BloodPressureRecord>>(
                $"/api/patient/{id}/measurement/blood-pressure");
            var fetchCholesterolTask = _http.GetFromJsonAsync<List<CholesterolRecord>>(
                $"/api/patient/{id}/measurement/cholesterol");
            await Task.WhenAll(fetchBloodPressureTask, fetchCholesterolTask);
            var bloodPressureRecords = await fetchBloodPressureTask;
            var cholesterolRecord = await fetchCholesterolTask;
            var measurement = new Measurement()
            {
                BloodPressureRecords = bloodPressureRecords,
                CholesterolRecords = cholesterolRecord
            };
            return measurement;
        }

        public void ModifyRecordState(Record record)
        {
            record.ChangeIsMonitoredValue();
            NotifyStateChanged();
        }

        public async Task Update()
        {
            if (Dashboard.Monitors != null && Dashboard.Monitors.Count > 1)
            {
                foreach (var m in Dashboard.Monitors)
                {
                    m.MeasurementList = await FetchMeasurement(m);
                }
                NotifyStateChanged();
            } 
        }
        public void SetTime(string value)
        {
            try
            {
                t.Interval = 1000 * 60 * int.Parse(value);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("Value must be an integer");
                throw;
            }
        }

       
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
