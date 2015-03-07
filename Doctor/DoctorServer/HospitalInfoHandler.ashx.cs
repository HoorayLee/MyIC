using Doctor.DAL;
using Doctor.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// HospitalInfoHandler 的摘要说明
    /// </summary>
    public class HospitalInfoHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //发过来医院的主键，返回医院的详细信息
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            long hospital_id = long.Parse(requestStr);
            HospitalModel hospital = HospitalDAL.GetById(hospital_id);

            string json = JsonConvert.SerializeObject(hospital);

            byte[] buf = Encoding.UTF8.GetBytes(json);
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