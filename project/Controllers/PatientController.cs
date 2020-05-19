using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FHIR_FIT3077.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult PatientDisplay()
        {
            return View();
        }
    }
}