using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FHIR_FIT3077.Models;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.Controllers
{
    public class PractitionerController : Controller
    {
        // GET practitioner
        public ActionResult PractitionerView(string id)
        {
            var client = new FhirClient("https://fhir.monash.edu/hapi-fhir-jpaserver/fhir/");
            client.PreferredFormat = ResourceFormat.Json;
            client.UseFormatParam = true;
            client.Timeout = 120000;

            Bundle result = client.Search<Encounter>(new string[]
            {
                "participant.identifier=http://hl7.org/fhir/sid/us-npi|" + id
            });

            Dictionary<string, string> patientHashMap = new Dictionary<string, string>();
            foreach (var e in result.Entry)
            {
                Encounter p = (Encounter)e.Resource;
                var patient = p.Subject.Reference;
                var patientId = patient.Split('/')[1];
                var patientName = p.Subject.Display;
                patientHashMap[patientId] = patientName;
            }

            foreach (KeyValuePair<string, string> pair in patientHashMap)
            {
                Console.WriteLine("ID: " + pair.Key);
                Console.WriteLine("Name: " + pair.Value);
            }

            ViewData["Message"] = patientHashMap;
            return View();
        }
    }
}