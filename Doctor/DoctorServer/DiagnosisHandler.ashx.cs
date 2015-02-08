using Doctor.DAL;
using Doctor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// DiagnosisHandler 的摘要说明
    /// </summary>
    public class DiagnosisHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            DiagnosisModel diagnosis = JsonConvert.DeserializeObject<DiagnosisModel>(requestStr);

            JObject jObj = new JObject();
            if(DiagnosisDAL.Insert(diagnosis))
            {
                jObj.Add("state", "success");
            }
            else
            {
                jObj.Add("state", "failed");
            }

            byte[] buf = Encoding.UTF8.GetBytes(jObj.ToString());
            context.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}