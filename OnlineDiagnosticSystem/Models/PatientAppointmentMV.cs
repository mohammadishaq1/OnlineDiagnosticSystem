using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineDiagnosticSystem.Models
{
    public class PatientAppointmentMV
    {
       
        public int LabAppointID { get; set; }
       
        public int LabTestDetailID { get; set; }
        public string DetailName { get; set; }

        public int PatientValue { get; set; }

        public int MinValue { get; set; }
     
        public int MaxValue { get; set; }
    }
}