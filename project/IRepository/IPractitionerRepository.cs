using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.IRepository
{
    public interface IPractitionerRepository
    {
        public Practitioner GetPractitioner(long id);
    }
}
