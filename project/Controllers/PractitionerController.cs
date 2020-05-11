using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.IRepository;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Repository;
using FHIR_FIT3077.ViewModel;
using FHIR_FIT3077.ViewModels;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FHIR_FIT3077.Controllers
{
    public class PractitionerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        private readonly IPractitionerRepository _practitioner;
        private readonly ICacheRepository _cache;

        public PractitionerController(IPractitionerRepository practitioner, ICacheRepository distributedCache)
        {
            _practitioner = practitioner;
            _cache = distributedCache;
        }

        [HttpPost]
        [ActionName("SubmitId")]
        public IActionResult SubmitId(PractitionerModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("LoadPatient", new {id = model.Id});
            }

            return Index();
        }


        //This method load all unique patients from the server and store the list in cache
        public IActionResult LoadPatient(string id)
        {
            var patientModel = new PatientViewModel();
            if (_cache.ExistObject<Dictionary<string, PatientModel>>(id.ToString()) == true)
            {
                var patientsFromCache = _cache.GetObject<Dictionary<string, PatientModel>>(id.ToString());
            }
            else
            {
                patientModel.PatientList = _practitioner.GetTotalPatients(id);
                _cache.SetObject("Patients", patientModel.PatientList);
            }
            return View(patientModel);
        }

        //This method deregisters a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult DeregisterPatient(string id)
        {
            var monitorModel = new MonitorViewModel();
            monitorModel.MonitorList = _practitioner.DeregisterPatient(id,
                _cache.GetObject<Dictionary<string, PatientModel>>("Monitor"));
            _cache.SetObject("Monitor", monitorModel.MonitorList);

            return PartialView(monitorModel);
        }

        //This method registers a patient from the monitor list stored in cache by id
        [HttpPost]
        public IActionResult RegisterPatient(string id)
        {
            var monitorModel = new MonitorViewModel();
            if (_cache.ExistObject<List<PatientModel>>("Monitor") == true)
            {
                monitorModel.MonitorList = _cache.GetObject<Dictionary<string, PatientModel>>("Monitor");
            }
            else
            {
                monitorModel.MonitorList = _practitioner.RegisterPatient(id,
                    _cache.GetObject<Dictionary<string, PatientModel>>(id.ToString()),
                    _cache.GetObject<Dictionary<string, PatientModel>>("Monitor"));
                _cache.SetObject("Monitor", monitorModel.MonitorList);
            }
            return PartialView(monitorModel);
        }

        
    }
}