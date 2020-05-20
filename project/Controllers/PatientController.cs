using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Observer;
using Microsoft.AspNetCore.Mvc;

namespace FHIR_FIT3077.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult PatientDisplay(MonitorModel model)
        {
            ViewData["id"] = model.Id;
            return View();
        }
    }
}