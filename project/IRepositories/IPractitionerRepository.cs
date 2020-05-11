using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.IRepository
{
    public interface IPractitionerRepository
    {
        Dictionary<string, PatientModel> GetPatient(string id);

        

    }
}
