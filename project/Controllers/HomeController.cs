using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetPractitioner(PractitionerModel model)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                message = "Practitioner" + model.Name + "found";
            }
            else
            {
                message = "Failed to find the practitioner. Please try again";
            }

            return Content(message);
        }
    }
}
