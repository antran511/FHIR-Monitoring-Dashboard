using System;
using System.Collections.Generic;
using FHIR_FIT3077.IRepository;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using FHIR_FIT3077.Models;

namespace FHIR_FIT3077.Repository
{
    public class PractitionerRepository : IPractitionerRepository
    {
        private static FhirClient _client;
        public static void InitializeClient()
        {
            _client = new FhirClient("https://fhir.monash.edu/hapi-fhir-jpaserver/fhir/");
            _client.Timeout = 120000;
        }

        public Dictionary<string, PatientModel> GetPatient(string id)
        {
            InitializeClient();
            var patientList = new Dictionary<string, PatientModel>();
            Bundle result = _client.Search<Encounter>(new string[]
            {
                "participant.identifier=http://hl7.org/fhir/sid/us-npi|" + id.ToString()
            });
            foreach (var e in result.Entry)
            {
                Encounter p = (Encounter) e.Resource;
                var res = p.Subject.Reference;
                var patientId = res.Split('/')[1];
                var patientName = p.Subject.Display;
                var patient = new PatientModel() { Id = patientId, Name = patientName };
                if (!patientList.ContainsKey(patientId))
                {
                    patientList.Add(patientId, patient);
                }
            }

            return(patientList);

        }

        public Dictionary<string, PatientModel> RegisterPatient(string id, Dictionary<string, PatientModel> patientList, Dictionary<string, PatientModel> monitorList)
        {
            if (!monitorList.ContainsKey(id))
            {
                monitorList.Add(id, patientList[id]);
            }
            else
            {
                monitorList[id] = patientList[id];
            }
            

            return (monitorList);
        }

        public Dictionary<string, PatientModel> DeregisterPatient(string id,
            Dictionary<string, PatientModel> monitorList)
        {
            if (monitorList.ContainsKey(id))
            {
                monitorList.Remove(id);
            }

            return (monitorList);
        }
    }
}