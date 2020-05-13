using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using FHIR_FIT3077.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIR_FIT3077.Repositories
{
    public class MonitorRepository : IMonitorRepository
    {
        private readonly ICacheRepository _cache;

        public MonitorRepository(ICacheRepository distributedCache)
        {
            _cache = distributedCache;
        }

        public PatientViewModel DeregisterPatient(string id)
        {
            if (_cache.ExistObject<List<PatientViewModel>>("Monitor") == true)
            {
                var monitorViewModel = new PatientViewModel();
                monitorViewModel.MonitorList = _cache.GetObject<List<MonitorModel>>("Monitor");
                var monitorToRemove = monitorViewModel.MonitorList.SingleOrDefault(monitor => monitor.Id == id);
                monitorViewModel.MonitorList.Remove(monitorToRemove);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);
                return monitorViewModel;
            }
            else
            {
                return null;
            }

        }

        public PatientViewModel RegisterPatient(string id)
        {
            var monitorViewModel = new PatientViewModel();
            var patientList = _cache.GetObject<Dictionary<string, PatientModel>>("Patients");
            var monitor = new MonitorModel();
            monitor.Subscribe(patientList[id]);
            monitor.OnNext(patientList[id]);
            if (_cache.ExistObject<List<PatientViewModel>>("Monitor") == true)
            {
                monitorViewModel.MonitorList = _cache.GetObject<List<MonitorModel>>("Monitor");
                monitorViewModel.MonitorList.Add(monitor);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);
            }
            else
            {
                monitorViewModel.MonitorList = new List<MonitorModel>();
                monitorViewModel.MonitorList.Add(monitor);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);

            }

            return monitorViewModel;
        }
    }
}
