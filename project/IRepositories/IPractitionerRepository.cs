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

        Dictionary<string, PatientModel> AddPatient(string id, Dictionary<string, PatientModel> patientList,
            Dictionary<string, PatientModel> monitorList);

        Dictionary<string, PatientModel> RemovePatient(string id, Dictionary<string, PatientModel> monitorList);

    }
}
