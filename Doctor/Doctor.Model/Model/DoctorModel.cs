using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor.Model
{
    public class DoctorModel
    {
        [JsonProperty("Doc_id")]
        public System.Int64 Doc_id { get; set; }

        [JsonProperty("Hospital_id")]
        public System.Int64? Hospital_id { get; set; }

        [JsonProperty("Password")]
        public System.String Password { get; set; }

        [JsonProperty("Name")]
        public System.String Name { get; set; }

        [JsonProperty("PhotoPath")]
        public System.String PhotoPath { get; set; }

        [JsonProperty("Hospital")]
        public System.String Hospital { get; set; }

        [JsonProperty("LicenseNo")]
        public System.String LicenseNo { get; set; }

        [JsonProperty("LicensePath")]
        public System.String LicensePath { get; set; }

        [JsonProperty("IfAuth")]
        public System.Boolean IfAuth { get; set; }

    }

}
