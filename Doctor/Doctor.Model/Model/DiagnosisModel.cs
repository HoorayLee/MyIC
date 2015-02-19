using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor.Model
{
    public class DiagnosisModel
    {
        public System.Int64 Diagnosis_id { get; set; }
        public System.Int64 Doc_id { get; set; }
        public System.Int64 Record_id { get; set; }
        public System.String Result { get; set; }
        public System.DateTime Time { get; set; }
    }

}
