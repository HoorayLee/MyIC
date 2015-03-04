using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor.Model
{
    public class IPRecord
    {
        [JsonProperty("ret")]
        public int Ret { get; set; }

        [JsonProperty("start")]
        public string StartIP { get; set; }

        [JsonProperty("end")]
        public string EndIP { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("isp")]
        public string ISP { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        public override string ToString()
        {
            return Country + Province + City + District;
        }
    }
}
