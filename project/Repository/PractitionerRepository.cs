using FHIR_FIT3077.IRepository;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.Repository
{
    public class PractitionerRepository : IPractitionerRepository
    {
        private readonly IPractitionerRepository _pracrepository;
        public PractitionerRepository(IPractitionerRepository pracrepository)
        {
            _pracrepository = pracrepository;
        }

        public Practitioner GetPractitioner(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}