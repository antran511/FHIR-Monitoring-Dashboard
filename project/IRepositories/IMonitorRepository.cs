using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using FHIR_FIT3077.ViewModels;

namespace FHIR_FIT3077.IRepositories
{
    public interface IMonitorRepository
    {
        List<PatientMonitorModel> AddMonitor(PatientMonitorModel monitor, MonitorViewModel monitorList);

        List<PatientMonitorModel> RemovePatient();
    }
}
