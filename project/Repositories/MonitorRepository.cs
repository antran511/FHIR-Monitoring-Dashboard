using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR_FIT3077.IRepositories;
using FHIR_FIT3077.Models;
using FHIR_FIT3077.Observer;
using FHIR_FIT3077.ViewModel;
using FHIR_FIT3077.ViewModels;

namespace FHIR_FIT3077.Repositories
{
    public class MonitorRepository : IMonitorRepository
    {
        public List<PatientMonitorModel> AddMonitor(PatientMonitorModel monitor, MonitorViewModel monitorList)
        {
            monitorList.MonitorList.Add(monitor);
            return monitorList;
        }

        public List<PatientMonitorModel> RemovePatient(string id,
            Dictionary<string, T> monitorList)
        {
            if (monitorList.ContainsKey(id))
            {
                monitorList.Remove(id);
            }

            return (monitorList);
        }

        public List<PatientMonitorModel> RemovePatient()
        {
            throw new NotImplementedException();
        }
    }
}
