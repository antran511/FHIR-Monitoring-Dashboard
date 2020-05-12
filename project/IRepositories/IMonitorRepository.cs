using FHIR_FIT3077.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIR_FIT3077.IRepositories
{
    interface IMonitorRepository
    {
        PatientViewModel DeregisterPatient(string id);
        PatientViewModel RegisterPatient(string id);
    }
}
