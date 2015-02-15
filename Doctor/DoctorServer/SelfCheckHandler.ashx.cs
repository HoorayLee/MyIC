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
    /// SelfCheckHandler 的摘要说明
    /// </summary>
    public class SelfCheckHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            if ("ListAll".Equals(requestStr))
            {
                //返回所有本地区的所有自检结果
                RecordModel[] recordModels = RecordDAL.GetAll();
                JObject jObj = new JObject();
                jObj.Add("count", recordModels.Length);
                JArray jArr = new JArray();
                foreach (RecordModel recordModel in recordModels)
                {
                    jArr.Add(JsonConvert.SerializeObject(recordModel));
                }
                jObj.Add("content", jArr);

                byte[] bytes = Encoding.UTF8.GetBytes(jObj.ToString());
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                //返回指定id对应的自检图片
                long id = long.Parse(requestStr);
                PhotoModel[] photos = PhotoDAL.GetAllByRecordId(id);
                JObject jObj = new JObject();
                jObj.Add("count", photos.Length);
                JArray jArr = new JArray();
                foreach (PhotoModel photo in photos)
                {
                    jArr.Add(JsonConvert.SerializeObject(photo));
                }
                jObj.Add("content", jArr);

                byte[] bytes = Encoding.UTF8.GetBytes(jObj.ToString());
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
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