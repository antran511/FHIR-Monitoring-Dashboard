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

        public event Action OnChange;

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
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task AddToMonitors(Patient patient)
        {
            Monitor monitor = new Monitor(patient.Id, patient.Name);
            monitor.MeasurementList = await FetchMeasurement(monitor);
            Dashboard.RegisterMonitor(monitor);
            NotifyStateChanged();
        }

        public void RemoveFromMonitors(string monitorId)
        {
            Dashboard.DeregisterMonitor(monitorId);
            NotifyStateChanged();
        }

        public async Task<Measurement> FetchMeasurement(Monitor monitor)
        {
            var id = monitor.PatientId;
            var fetchBloodPressureTask = _http.GetFromJsonAsync<List<BloodPressureRecord>>(
                $"/api/patient/{id}/measurement/blood-pressure");
            var fetchCholesterolTask = _http.GetFromJsonAsync<List<Record>>(
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
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
