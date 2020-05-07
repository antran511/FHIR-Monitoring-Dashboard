using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
namespace FHIR_FIT3077.Service
{
    public class PractitionerService :  ApiAbstractService
    {
        public FhirClient Client { get; private set; }

        public string GetPractitioner()
        {
            Client = InitializeClient();
            try
            {
                Patient patient = new Patient();
                patient.s
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
