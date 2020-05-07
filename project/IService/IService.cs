using Hl7.Fhir.Rest;

namespace FHIR_FIT3077.IService
{
    public interface IApiService
    {
        public FhirClient InitializeClient();
    }
}