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
        public IActionResult DeregisterPatient(string id, string pracId)
        {
            string cacheMonitorKey = "Monitor" + pracId;
            if (_cache.ExistObject<List<PatientViewModel>>(cacheMonitorKey) == true)
            {
                var monitorViewModel = new PatientViewModel
                {
                    MonitorList = _cache.GetObject<List<MonitorModel>>(cacheMonitorKey)
                };
                var monitorToRemove = monitorViewModel.MonitorList.SingleOrDefault(monitor => monitor.Id == id);
                monitorViewModel.MonitorList.Remove(monitorToRemove);
                _cache.SetObject(cacheMonitorKey, monitorViewModel.MonitorList);

                var max = GetHighCholesterol(monitorViewModel.MonitorList);
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
        public IActionResult RegisterPatient(string id, string pracId)
        {
            string cachePatientKey = "Patients" + pracId;
            string cacheMonitorKey = "Monitor" + pracId;
            var monitorViewModel = new PatientViewModel();
            var patientList = _cache.GetObject<Dictionary<string, PatientModel>>(cachePatientKey);
            var monitor = new MonitorModel();
            monitor.Subscribe(patientList[id]);
            monitor.OnNext(patientList[id]);
            if (_cache.ExistObject<List<PatientViewModel>>(cacheMonitorKey) == true)
            {
                var monitorList = _cache.GetObject<List<MonitorModel>>(cacheMonitorKey);

                bool notExist = true;
                for (int i = 0; i < monitorList.Count; i++)
                {
                    if (monitorList[i].Id == monitor.Id)
                    {
                        monitorList[i] = monitor;
                        notExist = false;
                    }
                }
                if (notExist)
                {
                    monitorList.Add(monitor);
                }
                
                monitorViewModel.MonitorList = monitorList;
                _cache.SetObject(cacheMonitorKey, monitorViewModel.MonitorList);
            }
            else
            {
                monitorViewModel.MonitorList = new List<MonitorModel>
                {
                    monitor
                };
                _cache.SetObject(cacheMonitorKey, monitorViewModel.MonitorList);

            }
            var max = GetHighCholesterol(monitorViewModel.MonitorList);
            ViewData["max"] = max;
            return PartialView("_MonitorSection",monitorViewModel);
        }

        public List<double> GetHighCholesterol(List<MonitorModel> monitorList)
        {
            double totalChol = 0.0;
            double averageChol;
            List<double> highCholVal = new List<double>();
            for (int i = 0; i < monitorList.Count; i++)
            {
                var value = Convert.ToDouble(monitorList[i].Records[0].Value);
                totalChol += value;
            }
            averageChol = totalChol / monitorList.Count;
            for (int j = 0; j < monitorList.Count; j++)
            {
                var value = Convert.ToDouble(monitorList[j].Records[0].Value);
                if (value > averageChol)
                {
                    highCholVal.Add(value);
                }
            }
            return highCholVal; 
        }
    }
}