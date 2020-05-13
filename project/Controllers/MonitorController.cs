using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using FHIR_FIT3077.Repositories;
using FHIR_FIT3077.ViewModel;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;

namespace FHIR_FIT3077.Controllers
{
    public class MonitorController : Controller
    {
        private readonly ICacheRepository _cache;
        public MonitorController( ICacheRepository distributedCache)
        {
            _cache = distributedCache;
        }

        //This method registers a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult DeregisterPatient(string id)
        {
            if (_cache.ExistObject<List<PatientViewModel>>("Monitor") == true)
            {
                var monitorViewModel = new PatientViewModel();
                monitorViewModel.MonitorList = _cache.GetObject<List<MonitorModel>>("Monitor");
                var monitorToRemove = monitorViewModel.MonitorList.SingleOrDefault(monitor => monitor.Id == id);
                monitorViewModel.MonitorList.Remove(monitorToRemove);
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);

                var max = GetMaxCholesterol(monitorViewModel.MonitorList);
                ViewData["max"] = max;
                return PartialView("_MonitorSection", monitorViewModel);
            }
            else
            {
                return null;
            }
            
        }

        //This method register a monitor of patient into monitor list
        [HttpPost]
        public IActionResult RegisterPatient(string id)
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
                monitorViewModel.MonitorList = new List<MonitorModel>
                {
                    monitor
                };
                _cache.SetObject("Monitor", monitorViewModel.MonitorList);

            }
            var max = GetMaxCholesterol(monitorViewModel.MonitorList);
            ViewData["max"] = max;
            return PartialView("_MonitorSection",monitorViewModel);
        }

        public double GetMaxCholesterol(List<MonitorModel> monitorList)
        {
            double max = 0.0;
            for (int i = 0; i < monitorList.Count; i++)
            {
                var value = Convert.ToDouble(monitorList[i].Records[0].Value);
                if ( value > max)
                {
                    max = value;
                }
            }
            return max; 
        }
    }
}