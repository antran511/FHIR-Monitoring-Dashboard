﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FHIR_FIT3077.Models
{
    public class Practitioner
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public List<Patients> PatientList { get; set; }
    }
}