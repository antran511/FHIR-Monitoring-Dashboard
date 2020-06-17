using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT3077.Shared.Models;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT3077.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IFhirClient _client;

        public PatientController()
        {
            _client = new FhirClient("https://fhir.monash.edu/hapi-fhir-jpaserver/fhir/");
        }

        [HttpGet("{id}/measurement/blood-pressure")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<BloodPressureRecord>> FetchBloodPressure(string id)
        {
            var q = new SearchParams().Where("patient=" + id).Where("code=55284-4").OrderBy("-date");
            q.Count = 5;
            Bundle result = await _client.SearchAsync<Observation>(q);
            var bloodPressureRecords = new List<BloodPressureRecord>();
            if (result.Total > 0)
            {
                List<Bundle.EntryComponent> entryList = result.Entry;
                foreach (var entry in entryList)
                {
                    Observation o = (Observation) entry.Resource;
                    var systolicRecord = new Record()
                    {
                        Type = RecordType.SystolicPressure,
                        Value =  ((Quantity)o.Component[1].Value).Value.ToString(),
                        Date = DateTime.Parse(o.Issued.ToFhirDateTime()).ToString("dd/MM/yyyy HH:mm")
                    };
                    var diastolicRecord = new Record()
                    {
                        Type = RecordType.SystolicPressure,
                        Value = ((Quantity)o.Component[0].Value).Value.ToString(),
                        Date = DateTime.Parse(o.Issued.ToFhirDateTime()).ToString("dd/MM/yyyy HH:mm")
                    };
                    bloodPressureRecords.Add(new BloodPressureRecord()
                    {
                        SystolicRecord = systolicRecord,
                        DiastolicRecord = diastolicRecord
                    });
                };
            }

            return bloodPressureRecords;


        }
        [HttpGet("{id}/measurement/cholesterol")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<Record>> FetchCholesterol(string id)
        {
            //Create search parameters
            var q = new SearchParams().Where("patient=" + id).Where("code=2093-3").OrderBy("-date");
            //Set number of results per page to 1
            q.Count = 1;
            //Search result using parameters above
            Bundle result = await _client.SearchAsync<Observation>(q);
            var cholesterolRecords = new List<Record>();
            if (result.Total > 0)
            {
                Bundle.EntryComponent e = (Bundle.EntryComponent)result.Entry[0];
                Observation o = (Observation)e.Resource;
                var record = new Record()
                {
                    Type = RecordType.Cholesterol,
                    Date = DateTime.Parse(o.Issued.ToFhirDateTime()).ToString("dd/MM/yyyy HH:mm"),
                    Value =  ((Quantity)o.Value).Value.ToString()
                };
                cholesterolRecords.Add(record);
            }
            return cholesterolRecords;


        }
    }

}
