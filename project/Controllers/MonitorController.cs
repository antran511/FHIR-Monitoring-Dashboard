using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.IRepository;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using FHIR_FIT3077.ViewModels;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;

namespace FHIR_FIT3077.Controllers
{
    public class MonitorController : Controller
    {
        private ICacheRepository _cache;
        private IMonitorRepository _monitor;

        public MonitorController(IMonitorRepository monitor, ICacheRepository distributedCache)
        {
            _monitor = monitor;
            _cache = distributedCache;
        }

        //This method registers a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult DeregisterPatient(string id)
        {
            var monitorViewModel = new MonitorViewModel();
            var patientList = _cache.GetObject<Dictionary<string, PatientModel>>(id.ToString());
            monitorViewModel.MonitorList = _cache.GetObject<List<PatientMonitorModel>>("Monitor");
            var monitorToRemove = monitorViewModel.MonitorList.SingleOrDefault(monitor => monitor.Id == id);
            monitorViewModel.MonitorList.Remove(monitorToRemove);
            _cache.SetObject("Monitor", monitorViewModel.MonitorList);
            return PartialView(monitorViewModel.MonitorList);

        }

        //This method deregisters a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult RegisterPatient(string id)
        {
            var monitorViewModel = new MonitorViewModel();
            var patientList = _cache.GetObject<Dictionary<string, PatientModel>>(id.ToString());
            var monitor = new PatientMonitorModel(patientList[id]);
            if (_cache.ExistObject<List<MonitorViewModel>>("Monitor") == true)
            {
                monitorViewModel.MonitorList = _cache.GetObject<List<PatientMonitorModel>>("Monitor");
                monitorViewModel.MonitorList.Add(monitor);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);
            }
            else
            {
                monitorViewModel.MonitorList.Add(monitor);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);

            }
            return PartialView(monitorViewModel.MonitorList);
        }
    }
}