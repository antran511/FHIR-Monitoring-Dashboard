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

                
                if (!patientList.ContainsKey(patientId))
                {
                    patientList.Add(patientId, patient);
                }
            }

            foreach (KeyValuePair<string, PatientModel> entry in patientList)
            {
                Bundle patientCholesterol = _client.Search<Observation>(new string[]
                {
                    "patient="+ $"{entry.Key}" +"&code=2093-3&_sort=date&_count=13"
                });
                try
                {
                    Console.WriteLine("code reach here");
                    foreach (var o in patientCholesterol.Entry)
                    {
                        Observation item = (Observation)o.Resource;
                        var cholesterolVal = item.Value.ToString();
                        var date = item.Issued.ToString();
                        var record = new RecordModel() { Cholesterol = cholesterolVal, Date = date };
                        Console.WriteLine(cholesterolVal);
                        Console.WriteLine(date + "\n");
                        entry.Value.Records.Add(record);
                    }
                }
                catch
                {
                    Console.WriteLine("No patient has cholesterol");
                }
            }
            
            return(patientList);
        }

        
    }
}