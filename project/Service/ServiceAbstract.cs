using FHIR_FIT3077.IService;
using Hl7.Fhir.Rest;

namespace FHIR_FIT3077.Service
{
    public abstract class ApiAbstractService : IApiService
    {
        public FhirClient InitializeClient()
        {
            var client = new FhirClient("https://fhir.monash.edu/hapi-fhir-jpaserver/fhir/");
            client.PreferredFormat = ResourceFormat.Json;
            client.UseFormatParam = true;
            client.Timeout = 120000;
            return client;
        }
    }
}
 