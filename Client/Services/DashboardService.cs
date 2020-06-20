using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using FIT3077.Shared.Models;

namespace FIT3077.Client.Services
{
    public class DashboardService : IDashboardService
    {
        private Dashboard Dashboard { get; } = new Dashboard();
        public IReadOnlyList<Monitor> Monitors => Dashboard.Monitors;

        public IReadOnlyDictionary<string, Patient> Patients => Dashboard.Patients;
        public Timer t => Dashboard.t;
        public bool SearchInProgress { get; private set; }
        public event Action OnChange;
        

        private readonly HttpClient _http;
        public DashboardService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task Search(InputParameter patientId)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Dashboard.FetchPatientList(
                await _http.GetFromJsonAsync<Dictionary<string, Patient>>($"/api/practitioner/{patientId.Value}"));
            Dashboard.ClearMonitor();
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task AddToMonitors(Patient patient)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Dashboard.RegisterMonitor(patient, await FetchMeasurement(patient.Id));
            SearchInProgress = false;
            NotifyStateChanged();
        }

        public void RemoveFromMonitors(Monitor monitor)
        {
            Dashboard.DeregisterMonitor(monitor);
            NotifyStateChanged();
        }

        private async Task<Measurement> FetchMeasurement(string id)
        {
            var fetchBloodPressureTask = _http.GetFromJsonAsync<List<BloodPressureRecord>>(
                $"/api/patient/{id}/measurement/blood-pressure");
            var fetchCholesterolTask = _http.GetFromJsonAsync<List<CholesterolRecord>>(
                $"/api/patient/{id}/measurement/cholesterol");
            await Task.WhenAll(fetchBloodPressureTask, fetchCholesterolTask);
            var measurement = new Measurement()
            {
                BloodPressureRecords = new BloodPressureList() { Records = await fetchBloodPressureTask },
                CholesterolRecords = new CholesterolList() { Records = await fetchCholesterolTask }
            };
            return measurement;
        }

        public void ModifyRecordState(RecordList recordList)
        {
            recordList.ChangeMeasurementMonitoreState();
            NotifyStateChanged();
        }

        public async Task Update()
        {
            if (Dashboard.Monitors != null && Dashboard.Monitors.Count > 1)
            {
                foreach (var m in Dashboard.Monitors)
                {
                    Dashboard.UpdateMeasurement(m, await FetchMeasurement(m.PatientId));
                }
                NotifyStateChanged();
            } 
        }
        public void SetTime(InputParameter timeInput)
        {
            try
            {
                t.Interval = 1000 * 60 * int.Parse(timeInput.Value);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("Value must be an integer");
                throw;
            }
        }

        public void ProcessHighBloodInput(SysDiastolicThreshold highBloodValues)
        {
            Dashboard.HighBloodPressureFlag(highBloodValues);
            NotifyStateChanged();
        }

        public void CreateTimer()
        {
            Dashboard.t = new Timer();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
