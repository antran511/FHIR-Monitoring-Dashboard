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

        public Dictionary<string, PatientModel> GetTotalPatients(string id)
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
>>>>>>> Repositories/PractitionerRepository.cs

                
                if (!patientList.ContainsKey(patientId))
                {
                    var q = new SearchParams().Where("patient=" + patientId).Where("code=2093-3").OrderBy("-date");
                    Bundle patientChol = _client.Search<Observation>(q);
                    foreach (var entry in patientChol.Entry)
                    {
                        Observation item = (Observation)entry.Resource;
                        var cholesterol = ((Hl7.Fhir.Model.Quantity)item.Value).Value;
                        var date = item.Issued.ToString();
                        var record = new RecordModel() { Cholesterol = cholesterol.ToString(), Date = date };
                        if (patient.Record == null)
                        {
                            patient.Record = record;
                        }

                    }
                    patientList.Add(patientId, patient);
                    Console.WriteLine(patientId + " " + patientName);
                    Console.WriteLine(patient.Record.Cholesterol);
                    Console.WriteLine(patient.Record.Date + "\n");
                }
            }
<<<<<<< Repositories/PractitionerRepository.cs
            
            return(patientList);

        }

        public Dictionary<string, PatientModel> RegisterPatient(string id, Dictionary<string, PatientModel> patientList, Dictionary<string, PatientModel> monitorList)
        {
            if (!monitorList.ContainsKey(id))
            {
                monitorList.Add(id, patientList[id]);
            }
            
            return (monitorList);
=======
>>>>>>> Repositories/PractitionerRepository.cs
        }
    }
}