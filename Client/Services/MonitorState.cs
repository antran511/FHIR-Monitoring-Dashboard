using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FIT3077.Shared.Models;

namespace FIT3077.Client.Services
{
    public class MonitorState
    {
        public IReadOnlyDictionary<string, Patient> Patients { get; private set; }
        public bool SearchInProgress { get; private set; }

        public event Action OnChange;

        private readonly HttpClient _http;
        public MonitorState(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task Search(string id)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Patients = await _http.GetFromJsonAsync<Dictionary<string, Patient>>($"/api/practitioner/{id}");
            SearchInProgress = false;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
