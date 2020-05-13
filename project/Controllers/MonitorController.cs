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
        private ICacheRepository _cache;
        private IMonitorRepository _monitor;

        public MonitorController( ICacheRepository distributedCache)
        {
            _cache = distributedCache;
        }

        //This method registers a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult DeregisterPatient(string id)
        {
            if (_cache.ExistObject<List<MonitorModel>>("MonitorList") == true)
            {
                var monitorViewModel = new PatientViewModel();
                monitorViewModel.MonitorList = _cache.GetObject<List<MonitorModel>>("MonitorList");
                var monitorToRemove = monitorViewModel.MonitorList.SingleOrDefault(monitor => monitor.Id == id);
                monitorViewModel.MonitorList.Remove(monitorToRemove);
                _cache.SetObject("MonitorList", monitorViewModel.MonitorList);
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
            var monitorViewModel = _monitor.RegisterPatient(id);
            
            return PartialView("_MonitorSection",monitorViewModel);
        }
    }
}