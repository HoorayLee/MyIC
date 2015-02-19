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
    /// 1. 如果发过来的是long型的整数：获取对自检编号为该整数的医生所有意见
    /// 2. 如果不是long型的整数：为新添加的医生意见
    /// </summary>
    public class DiagnosisHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            long record_id;
            JObject jObj = new JObject();   
            if (long.TryParse(requestStr, out record_id))
            {
                //获取对自检编号为该整数的医生所有意见
                DiagnosisModel[] diagnoses = DiagnosisDAL.GetAllByRecordId(record_id);
                int nbDiagnoses = diagnoses.Length;
                jObj.Add("count", nbDiagnoses);
                JArray jArr = new JArray();
                for (int i = 0; i < nbDiagnoses; i++)
                {
                    DiagnosisModel diagnosis = diagnoses[i];
                    DoctorModel doctor = DoctorDAL.GetById(diagnosis.Doc_id);
                    JObject jArrObj = new JObject();
                    jArrObj.Add("realname", doctor.RealName);
                    jArrObj.Add("time", diagnosis.Time);
                    jArrObj.Add("comment", diagnosis.Result);
                    jArr.Add(jArrObj);
                }
                jObj.Add("content", jArr);
            }
            else
            {
                //添加医生意见
                DiagnosisModel diagnosis = JsonConvert.DeserializeObject<DiagnosisModel>(requestStr);
                if (DiagnosisDAL.Insert(diagnosis))
                {
                    jObj.Add("state", "success");
                }
                else
                {
                    jObj.Add("state", "failed");
                }
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