using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorServer
{
    /// <summary>
    /// PicDownloadHandler 的摘要说明
    /// </summary>
    public class PicDownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StreamReader reader = new StreamReader(context.Request.InputStream);
            string fileName = reader.ReadToEnd();

            context.Response.WriteFile(Path.Combine(context.Server.MapPath("~/UploadFiles/"), fileName));
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