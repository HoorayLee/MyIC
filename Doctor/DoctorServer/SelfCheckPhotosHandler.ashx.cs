using Doctor.DAL;
using Doctor.Model;
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
    /// SelfCheckPhotosHandler 的摘要说明
    /// </summary>
    public class SelfCheckPhotosHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);
            string requestStr = reader.ReadToEnd();

            long record_id = long.Parse(requestStr);
            PhotoModel[] photos = PhotoDAL.GetAllByRecordId(record_id);

            JObject jObj = new JObject();
            jObj.Add("count", photos.Length);
            JArray jArr = new JArray();
            foreach (var photo in photos)
            {
                jArr.Add(photo.Path);
            }
            jObj.Add("content", jArr);

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